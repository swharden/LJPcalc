using LJPmath;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        public void Calculate(List<Ion> ionSet)
        {
            if (ionSet.Count < 2)
            {
                ResultLabel.Content = "Invalid ion set...";
                DetailText.Text = $"At least 2 ions are required to calculate LJP";
                return;
            }

            Ion secondFromLastIon = ionSet[ionSet.Count - 2];
            if (secondFromLastIon.c0 == secondFromLastIon.cL)
            {
                ResultLabel.Content = "Invalid ion set...";
                DetailText.Text = $"Second from last ion cannot have equal C0 and CL";
                return;
            }

            try
            {
                // do this to not modify the original ion set
                List<Ion> clonedIonSet = new List<Ion>();
                foreach (Ion ion in ionSet)
                    clonedIonSet.Add(new Ion(ion));

                // perform math on the cloned set (mutates it)
                double ljp = LJPmath.Calculate.Ljp(clonedIonSet);

                if (double.IsNormal(ljp))
                {
                    ResultLabel.Content = $"LJP = {ljp * 1000:0.000} mV";
                    DetailText.Text = "values produced during calculation:\r\n";
                    foreach (Ion ion in clonedIonSet)
                        DetailText.Text += $"{ion}\r\n";
                }
                else
                {
                    ResultLabel.Content = "ERROR";
                    DetailText.Text = $"LJP cannot be calculated from this combination of ions.";
                }
            }
            catch (Exception ex)
            {
                ResultLabel.Content = "ERROR";
                DetailText.Text = $"LJP cannot be calculated from this combination of ions.\n\n{ex}";
            }
        }
    }
}
