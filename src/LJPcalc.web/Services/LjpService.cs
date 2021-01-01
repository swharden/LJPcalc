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
        private readonly List<Ion> IonList = new List<Ion>();

        public string ResultText { get; private set; }
        public double ResultLJP { get; private set; }

        public string Version =>
            typeof(Ion).Assembly.GetName().Version.Major + "." +
            typeof(Ion).Assembly.GetName().Version.Minor;

        public void AddIon(string name, int charge, double conductivity, double concA, double concB) =>
            IonList.Add(new Ion(name, charge, conductivity, concA, concB));

        public void Clear() => IonList.Clear();

        public void RemoveAt(int index) => IonList.RemoveAt(index);

        public void CalculateLJP(double temperatureC)
        {
            var result = Calculate.Ljp(IonList, temperatureC);
            ResultText = result.report;
            ResultLJP = result.mV;
        }
    }
}
