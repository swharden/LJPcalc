using LJPcalc.web.InputModels;
using LJPmath;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
namespace LJPcalc.web.Services
{
    public class LjpService
    {
        public Action OnSolutionLabelChange;
        public Action OnSelectedIonChange;

        public readonly List<SampleIonSet> SampleIonSets = new List<SampleIonSet>();
        public readonly List<UserIon> IonList = new List<UserIon>();
        public readonly UserIon IonToAdd = new UserIon();
        public readonly UserTemperature Temperature = new UserTemperature();

        public string SelectedIonName
        {
            get => IonToAdd.Name.Input;
            set
            {
                var ion = KnownIons.GetIon(value);
                IonToAdd.Name.Input = ion.name;
                IonToAdd.Charge.Input = ion.charge.ToString();
                IonToAdd.Mobility.Input = ion.muE11.ToString();
                OnSelectedIonChange?.Invoke();
            }
        }

        public void AddSelectedIon()
        {
            IonList.Add(IonToAdd.Copy());
            LoadSampleSet(null);
        }

        public void RemoveIon(UserIon ion)
        {
            IonList.Remove(ion);
            LoadSampleSet(null);
        }

        public string LabelType;
        public string ServerType = "Server";
        public bool UseGenericLabels => LabelType == "generic";

        public event Action OnNewResult;
        public double ResultLJP = double.NaN;
        public Ion[] ResultIons;
        public string ResultDetails;
        public double ResultSeconds;
        public string ResultErrorMessage;
        public bool ResultCalculating;
        public string ResultSummarized
        {
            get
            {
                if (double.IsNaN(ResultLJP))
                    return null;

                string direction = ResultLJP > 0 ? "positive" : "negative";

                return UseGenericLabels ?
                    $"Solution B is {direction} relative to Solution A" :
                    $"The bath solution is {direction} relative to the pipette solution";
            }
        }

        public bool IsValidIonList => IonList.All(x => x.IsValid);

        public readonly IonLookup KnownIons = new IonLookup();

        public LjpService()
        {
            var knownSets = new KnownIonSets(KnownIons.Table);
            foreach (var knownSet in knownSets.ionSets)
                SampleIonSets.Add(SampleIonSet.FromKnownSet(knownSet));
            LoadDefaultSet();
        }

        public void LoadDefaultSet() => LoadSampleSet(SampleIonSets.Last());

        public string SampleSetTitle;
        public string SampleSetDescription;
        public void LoadSampleSet(SampleIonSet sampleSet)
        {
            if (sampleSet is null)
            {
                SampleSetTitle = null;
                SampleSetDescription = null;
                return;
            }

            IonList.Clear();
            IonList.AddRange(sampleSet.Ions);
            SampleSetTitle = sampleSet.Title;
            SampleSetDescription = sampleSet.Description + " This ion set is expected to have a " +
               $"LJP near near {sampleSet.LjpMV} mV at {sampleSet.TemperatureC} C.";
        }

        public string Version =>
            typeof(Ion).Assembly.GetName().Version.Major + "." +
            typeof(Ion).Assembly.GetName().Version.Minor;

        public async Task CalculateLJPAsync()
        {
            ResultLJP = double.NaN;
            ResultDetails = null;
            ResultErrorMessage = null;
            ResultIons = null;

            if (IonList.Count < 2)
            {
                ResultErrorMessage = "There must be at least 2 ions to calculate LJP";
                return;
            }

            if (IonList.Any(x => x.Charge.Charge == 0))
            {
                ResultErrorMessage = "Molecules without charge must be removed from the table. They are not ions!";
                return;
            }

            if (IonList.Any(x => x.C0.Concentration == 0 && x.CL.Concentration == 0))
            {
                ResultErrorMessage = "Ions with no concentration on both sides must be removed from the table.";
                return;
            }

            int LeftCations = IonList.Where(x => x.C0.Concentration > 0 && x.Charge.Charge > 0).Count();
            int RightCations = IonList.Where(x => x.C0.Concentration > 0 && x.Charge.Charge > 0).Count();
            int LeftAnions = IonList.Where(x => x.C0.Concentration > 0 && x.Charge.Charge < 0).Count();
            int RightAnions = IonList.Where(x => x.C0.Concentration > 0 && x.Charge.Charge < 0).Count();

            if (LeftCations == 0 || RightCations == 0)
            {
                ResultErrorMessage = "Each solution must contain at least one positive ion";
                return;
            }

            if (LeftAnions == 0 || RightAnions == 0)
            {
                ResultErrorMessage = "Each solution must contain at least one negative ion";
                return;
            }

            if (!IsValidIonList)
            {
                ResultErrorMessage = "All ions in the table must be valid. Invalid ions: " +
                    string.Join(",", IonList.Where(x => !x.IsValid).Select(x => x.Name.Input));
                return;
            }

            HashSet<string> seen = new HashSet<string>();
            foreach (string ionName in IonList.Select(x => x.Name.Input))
            {
                if (seen.Contains(ionName))
                {
                    ResultErrorMessage = $"{ionName} appears multiple times in the ion list";
                    return;
                }
                seen.Add(ionName);
            }

            if (ServerType == "Server")
                await CalculateLjpRemotelyAsync();
            else
                CalculateLjplocally();
        }

        private void CalculateLjplocally(int timeoutSec = 30)
        {
            try
            {
                Ion[] ions = IonList.Select(x => x.ToIon()).ToArray();
                var result = Calculate.Ljp(ions, Temperature.TemperatureC, timeoutMilliseconds: timeoutSec * 1e3);
                ResultLJP = result.mV;
                ResultDetails = result.report;
                ResultSeconds = result.benchmark_s;
                ResultIons = result.ionListSolved.ToArray();
            }
            catch (OperationCanceledException)
            {
                ResultErrorMessage = $"ERROR: The solver was unable to calculate LJP within {timeoutSec} seconds. " +
                    "To reduce complexity of the system consider removing ions with small concentrations that " +
                    "contribute little to the overall LJP. Alternatively, consider using the desktop application, " +
                    "as it is significantly more powerful than LJPcalc running in the browser.";
            }
            catch (Exception ex)
            {
                ResultErrorMessage = ex.ToString();
            }

            ResultCalculating = false;
            OnNewResult?.Invoke();
        }

        private async Task CalculateLjpRemotelyAsync()
        {
            try
            {
                // design an experiment and encode it as JSON
                Ion[] ions = IonList.Select(x => x.ToIon()).ToArray();
                var exp = new Experiment(ions, temperatureC: 25);
                string txJson = exp.ToJson();
                Console.WriteLine(txJson);

                // execute the HTTP request, get the response, and update the experiment
                string functionKey = "MiPqBqy0Bv0EYQ1QslBBgyMIX6qeeutZFJ27rJC9H/3ObKooolIfYQ==";
                string url = "https://ljpcalcapi.azurewebsites.net/api/CalculateLJP?code=" + functionKey;
                var data = new StringContent(txJson, Encoding.UTF8, "application/json");
                var client = new HttpClient();
                var response = await client.PostAsync(url, data);
                string rxJson = response.Content.ReadAsStringAsync().Result;

                System.Threading.Thread.Sleep(500);
                Console.WriteLine("RX: " + rxJson);

                exp.AddResultsJson(rxJson);

                // update things
                ResultLJP = exp.LjpMillivolts;
                ResultDetails = exp.GetReport();
                ResultSeconds = exp.CalculationSeconds;

                ResultIons = exp.SolvedIons;
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Forbidden)
                    ResultErrorMessage = $"This website is not permitted to make API calls to LJPcalc API. {ex}";
                else
                    ResultErrorMessage = ex.ToString();
            }
            catch (Exception ex)
            {
                ResultErrorMessage = ex.ToString();
            }

            ResultCalculating = false;
            OnNewResult?.Invoke();
        }
    }
}
