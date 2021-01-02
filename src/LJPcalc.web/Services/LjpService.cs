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

        public readonly KnownIons KnownIons = new KnownIons();
        public readonly List<Ion> IonList;
        public string AddIonName;
        public int AddIonCharge;
        public double AddIonMobility
        {
            get => Ion.Mobility(AddIonConductivity, AddIonCharge);
            set { AddIonConductivity = Ion.Conductivity(value, AddIonCharge); }
        }
        public double AddIonConductivity;
        public void AddIon() => IonList.Add(new Ion(AddIonName, AddIonCharge, AddIonConductivity, 0, 0));

        public string ErrorMessage;
        public bool HasError => !string.IsNullOrWhiteSpace(ErrorMessage);
        public bool UseGenericLabels = false;

        public string ResultText { get; private set; }
        public double ResultLJP { get; private set; }

        public bool IsValidIonList { get; private set; }
        public bool IsValidResult { get; private set; }

        public double TemperatureC = 25;

        public LjpService()
        {
            var samples = new SampleIonSet();
            IonList = samples.PotassiumGluconate.Ions;
            IsValidIonList = true;
        }

        public string Version =>
            typeof(Ion).Assembly.GetName().Version.Major + "." +
            typeof(Ion).Assembly.GetName().Version.Minor;

        public void CalculateLJP()
        {
            var result = Calculate.Ljp(IonList, TemperatureC, timeoutMilliseconds: 25_000);
            IsValidResult = !double.IsNaN(result.mV);
            ResultLJP = result.mV;
            ResultText = result.report;
        }
    }
}
