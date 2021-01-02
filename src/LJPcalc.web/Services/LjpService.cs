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

        public string ResultText { get; private set; }
        public double ResultLJP { get; private set; }

        public bool IsValidIonList => IonList.All(x => x.IsValid);
        public bool IsValidResult { get; private set; }

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
            var ions = IonList.Select(x => x.ToIon()).ToList();
            var result = Calculate.Ljp(ions, Temperature.TemperatureC, timeoutMilliseconds: 25_000);
            IsValidResult = !double.IsNaN(result.mV);
            ResultLJP = result.mV;
            ResultText = result.report;
        }
    }
}
