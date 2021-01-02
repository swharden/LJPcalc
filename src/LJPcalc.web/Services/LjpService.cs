using LJPcalc.web.InputModels;
using LJPmath;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LJPcalc.web.Services
{
    public class LjpService
    {
        public Action OnSolutionLabelChange;
        public Action OnSelectedIonChange;
        public Action OnIonTableChange;

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

        public void AddSelectedIon() => IonList.Add(IonToAdd.Copy());

        public string LabelType;
        public bool UseGenericLabels => LabelType == "generic";

        public double ResultLJP = double.NaN;
        public string ResultDetails;
        public string ResultErrorMessage;

        public bool IsValidIonList => IonList.All(x => x.IsValid);

        public readonly IonLookup KnownIons = new IonLookup();

        public LjpService()
        {
            var samples = new SampleIonSet();
            IonList.AddRange(samples.PotassiumGluconate.Ions);
        }

        public string Version =>
            typeof(Ion).Assembly.GetName().Version.Major + "." +
            typeof(Ion).Assembly.GetName().Version.Minor;

        public void CalculateLJP()
        {
            ResultLJP = double.NaN;
            ResultDetails = null;
            ResultErrorMessage = null;

            if (!IsValidIonList)
            {
                ResultErrorMessage = "All ions in the table must be valid";
                return;
            }

            if (IonList.Count < 2)
            {
                ResultErrorMessage = "There must be at least 2 ions to calculate LJP";
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

            try
            {
                var ions = IonList.Select(x => x.ToIon()).ToList();
                var result = Calculate.Ljp(ions, Temperature.TemperatureC, timeoutMilliseconds: 25_000);
                ResultLJP = result.mV;
                ResultDetails = result.report;
            }
            catch (Exception ex)
            {
                ResultErrorMessage = ex.ToString();
            }
        }
    }
}
