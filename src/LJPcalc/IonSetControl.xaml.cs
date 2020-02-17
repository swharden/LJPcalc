using LJPmath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for IonSetControl.xaml
    /// </summary>
    public partial class IonSetControl : UserControl
    {
        public readonly List<Ion> ionSet = new List<Ion>();
        public event EventHandler IonSetChanged = delegate { };
        public event EventHandler AboutButtonClicked = delegate { };

        public IonSetControl()
        {
            InitializeComponent();
            dataGrid1.ItemsSource = ionSet;
            UpdateGuiFromIonSet();
        }

        public void AddIon(Ion ion)
        {
            ionSet.Add(ion);
            UpdateGuiFromIonSet();
        }

        public void ClearIons()
        {
            ionSet.Clear();
            UpdateGuiFromIonSet();
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            ionSet.RemoveAt(dataGrid1.SelectedIndex);
            UpdateGuiFromIonSet();
        }

        private void MoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int indexCut = dataGrid1.SelectedIndex;
            int indexPaste = indexCut - 1;

            Ion ion = ionSet[indexCut];
            ionSet.RemoveAt(indexCut);
            ionSet.Insert(indexPaste, ion);

            UpdateGuiFromIonSet();
        }

        private void MoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int indexCut = dataGrid1.SelectedIndex;
            int indexPaste = indexCut + 1;

            Ion ion = ionSet[indexCut];
            ionSet.RemoveAt(indexCut);
            ionSet.Insert(indexPaste, ion);

            UpdateGuiFromIonSet();
        }

        private void UpdateGuiFromIonSet(bool refresh = true)
        {
            if (refresh)
                dataGrid1.Items.Refresh();
            dataGrid1_SelectionChanged(null, null);
            IonSetChanged(ionSet, EventArgs.Empty);
        }

        private void dataGrid1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isItemSelected = (dataGrid1.SelectedIndex >= 0);
            bool lastItemSelected = (dataGrid1.SelectedIndex == dataGrid1.Items.Count - 1);
            bool firstItemSelected = (dataGrid1.SelectedIndex == 0);

            MoveUpButton.IsEnabled = isItemSelected && !firstItemSelected;
            MoveDownButton.IsEnabled = isItemSelected && !lastItemSelected;
            RemoveButton.IsEnabled = isItemSelected;
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            MoreButton.ContextMenu.IsOpen = true;
        }

        private void ExampleButton_Click(object sender, RoutedEventArgs e)
        {
            ExampleButton.ContextMenu.IsOpen = true;
        }

        private void LoadExample_JLJP(object sender, RoutedEventArgs e)
        {
            // values from JLJP screenshot: https://a.fsdn.com/con/app/proj/jljp/screenshots/GUI.png/max/max/1.jpg
            var ionTable = new IonTable();
            ionSet.Clear();
            ionSet.Add(ionTable.Lookup(new Ion("Zn", 9, 2.84E-2)));
            ionSet.Add(ionTable.Lookup(new Ion("K", 0, 3)));
            ionSet.Add(ionTable.Lookup(new Ion("Cl", 18, 3.062)));
            UpdateGuiFromIonSet();
        }

        private void LoadExample_NgAndBarry(object sender, RoutedEventArgs e)
        {
            // values from Ng and Barry (1994) Table 2: https://doi.org/10.1016/0165-0270(94)00087-W
            var ionTable = new IonTable();
            ionSet.Clear();
            ionSet.Add(ionTable.Lookup(new Ion("Ca", 50, 0)));
            ionSet.Add(ionTable.Lookup(new Ion("Cl", 200, 100)));
            ionSet.Add(ionTable.Lookup(new Ion("Mg", 50, 0)));
            ionSet.Add(ionTable.Lookup(new Ion("Li", 0, 100)));
            UpdateGuiFromIonSet();
        }

        private void LoadExample_JPWin(object sender, RoutedEventArgs e)
        {
            // ion set shown in JPCalcWin manual (page 7): https://tinyurl.com/wk7otn7
            var ionTable = new IonTable();
            ionSet.Clear();
            ionSet.Add(ionTable.Lookup(new Ion("Na", 10, 145)));
            ionSet.Add(ionTable.Lookup(new Ion("Cl", 10, 145)));
            ionSet.Add(ionTable.Lookup(new Ion("Cs", 135, 0)));
            ionSet.Add(ionTable.Lookup(new Ion("F", 135, 0)));
            UpdateGuiFromIonSet();
        }

        private void LaunchBrowser_CalculationNotes(object sender, RoutedEventArgs e)
        {
            LaunchBrowser("https://github.com/swharden/LJPcalc#theory");
        }

        private void LaunchBrowser_LJPcalc(object sender, RoutedEventArgs e)
        {
            LaunchBrowser("https://github.com/swharden/LJPcalc");
        }
        

        private void LaunchBrowser(string url)
        {
            // A cross-platform .NET-Core-safe function to launch a URL in the browser
            Console.WriteLine($"Launching: {url}");
            try
            {
                Process.Start(url);
            }
            catch
            {
                if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}") { CreateNoWindow = true });
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
                    Process.Start("xdg-open", url);
                else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX))
                    Process.Start("open", url);
                else
                    throw;
            }
        }

        private void OnAboutButtonClicked(object sender, RoutedEventArgs e)
        {
            AboutButtonClicked(null, EventArgs.Empty);
        }

        private void OnClearIonSet(object sender, RoutedEventArgs e)
        {
            ClearIons();
        }

        private void dataGrid1_CurrentCellChanged(object sender, EventArgs e)
        {
            UpdateGuiFromIonSet(refresh: false);
        }
    }
}
