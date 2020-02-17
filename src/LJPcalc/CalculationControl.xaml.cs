using LJPmath;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace LJPcalc
{
    /// <summary>
    /// Interaction logic for CalculationControl.xaml
    /// </summary>
    public partial class CalculationControl : UserControl
    {
        public CalculationControl()
        {
            InitializeComponent();
        }

        List<Ion> clonedIonSet;
        public double result = 0;
        public string ionTableText;
        public string resultMessage;

        private void Calculate()
        {
            try
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                result = LJPmath.Calculate.Ljp(clonedIonSet);
                double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;
                ionTableText = "";
                foreach (Ion ion in clonedIonSet)
                    ionTableText += $"{ion}\r\n";
                resultMessage = string.Format("LJP = {1} mV (solved in {0:0.000} ms)", elapsedSec * 1000.0, result * 1000.0);
            }
            catch (Exception ex)
            {
                resultMessage = $"LJP cannot be calculated from this combination of ions.\n\n{ex}";
            }
        }

        public void DisplayResult()
        {
            ResultLabel.Content = (double.IsNormal(result)) ? $"LJP = {result * 1000:0.000} mV" : "ERROR";
            DetailText.Text = ionTableText + resultMessage;
        }


        public void Calculate(List<Ion> ionSet)
        {
            clonedIonSet = new List<Ion>();
            foreach (Ion ion in ionSet)
                clonedIonSet.Add(new Ion(ion));

            if (ionSet.Count < 2)
            {
                ResultLabel.Content = "Incomplete ion set";
                DetailText.Text = $"At least 2 ions are required to calculate LJP";
                return;
            }

            bool isAllAnions = true;
            bool isAllCations = true;
            foreach (Ion ion in ionSet)
            {
                if (ion.charge < 0)
                    isAllCations = false;
                if (ion.charge > 0)
                    isAllAnions = false;
            }

            if (isAllAnions || isAllCations)
            {
                ResultLabel.Content = "Unbalanced ion set";
                DetailText.Text = $"The ion set must contain both anions and cations";
                return;
            }

            Ion secondFromLastIon = ionSet[ionSet.Count - 2];
            if (secondFromLastIon.c0 == secondFromLastIon.cL)
            {
                ResultLabel.Content = "Invalid ion set";
                DetailText.Text = $"Second from last ion cannot have equal C0 and CL";
                return;
            }

            ResultLabel.Content = "Calculating...";
            DetailText.Text = $"";
            foreach (Ion ion in clonedIonSet)
                DetailText.Text += $"{ion}\r\n";

            var task = new Task(Calculate);
            task.ContinueWith((Calculate) => DisplayResult(), TaskScheduler.FromCurrentSynchronizationContext());
            task.Start();

        }
    }
}
