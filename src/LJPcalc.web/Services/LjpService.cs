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
            IonList = new List<Ion>
            {
                IonTable.GetIon("Ca", 50, 0),
                IonTable.GetIon("Cl", 200, 100),
                IonTable.GetIon("Mg", 50, 0),
                IonTable.GetIon("Li", 0, 100)
            };

            IsValidIonList = true;
        }

        public string Version =>
            typeof(Ion).Assembly.GetName().Version.Major + "." +
            typeof(Ion).Assembly.GetName().Version.Minor;

        public void CalculateLJP()
        {
            var result = Calculate.Ljp(IonList, TemperatureC);
            IsValidResult = !double.IsNaN(result.mV);
            ResultLJP = result.mV;
            ResultText = result.report;
        }
    }
}
