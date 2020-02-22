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
    /// Interaction logic for LjpCalcControl.xaml
    /// </summary>
    public partial class LjpCalcControl : UserControl
    {
        IonTable ionTable; // TODO: verify this is not null when accessed
        List<Ion> ionSet = new List<Ion>();

        public LjpCalcControl()
        {
            InitializeComponent();
            LoadIonTable();
            dataGrid1.ItemsSource = ionSet;
            ValidateIonSet();
            LoadExample_JLJP(null, null);
        }

        public void Message(string title, string details)
        {
            LjpTitle.Text = title;
            LjpDetails.Text = details;
        }

        #region ion table

        private void LoadIonTable()
        {
            IonTableListbox.Items.Clear();

            try
            {
                ionTable = new IonTable();
                foreach (var ion in ionTable.ions)
                    IonTableListbox.Items.Add(ion.nameWithCharge);

                // pre-select an item
                IonTableListbox.SelectedItem = IonTableListbox.Items[0];
                IonTableListbox.ScrollIntoView(IonTableListbox.SelectedItem);
            }
            catch
            {
                IonTableListbox.Items.Add("ERROR");
                IonTableListbox.IsEnabled = false;
            }
        }

        private void IonTableListboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IonTableListbox is null || ionTable is null || IonTableListbox.SelectedIndex < 0)
                return;

            string selectedIonName = IonTableListbox.SelectedItem.ToString();
            Ion ion = ionTable.Lookup(selectedIonName);

            if (ion != null)
            {
                IonNameTextbox.Text = ion.name;
                IonChargeTextbox.Text = ion.charge.ToString();
                IonConductivityTextbox.Text = (ion.conductivity * 1e4).ToString("0.000");
                IonC0Textbox.Text = "0";
                IonCLTextbox.Text = "0";
                Message($"Loaded Ion: {ion.nameWithCharge}", ion.ToString());
            }

            ValidateIon();
        }

        private void IonNameTextboxTextChanged(object sender, TextChangedEventArgs e)
        {
            // do NOT modify textbox contents while typing there. It's distracting to the user.
            Message("", "");
            ValidateIon();
        }

        private void IonTableDoubleClicked(object sender, MouseButtonEventArgs e)
        {
            if (IonAddButton.IsEnabled)
                AddIon(null, null);
        }

        #endregion

        #region ion creation

        private void IonLoadByUserEnteredName(object sender, RoutedEventArgs e)
        {
            if (ionTable is null)
                return;

            Ion ion = ionTable.Lookup(IonNameTextbox.Text);
            for (int i = 0; i < IonTableListbox.Items.Count; i++)
            {
                if (IonTableListbox.Items[i].ToString() == ion.nameWithCharge)
                {
                    IonTableListbox.SelectedIndex = i;
                    IonTableListbox.ScrollIntoView(IonTableListbox.Items[i]);
                    return;
                }
            }
        }

        private Ion GetIonFromUserValues()
        {
            try
            {
                int charge = int.Parse(IonChargeTextbox.Text);
                if (charge == 0) { Message("Invalid Ion", "Ion charge cannot be zero"); return null; }

                double conductivity = double.Parse(IonConductivityTextbox.Text) * 1e-4;
                if (conductivity <= 0) { Message("Invalid Ion", "Conductivity must be greater than zero"); return null; }

                double c0 = double.Parse(IonC0Textbox.Text);
                if (c0 < 0) { Message("Invalid Ion", "Concentrations must be greater than zero"); return null; }

                double cL = double.Parse(IonCLTextbox.Text);
                if (cL < 0) { Message("Invalid Ion", "Concentrations must be greater than zero"); return null; }

                Ion ion = new Ion(IonNameTextbox.Text, charge, conductivity, c0, cL);
                return ion;
            }
            catch { }

            return null;
        }

        private void ValidateIon(object sender, KeyEventArgs e) { ValidateIon(); }

        private void ValidateIon(object sender, TextChangedEventArgs e) { ValidateIon(); }

        private void ValidateIon()
        {
            if (IonAddButton is null || ionTable is null)
                return;

            if (IonNameTextbox.Text == "") { Message("Invalid Ion", "Ion name cannot be blank"); return; }

            bool userEnteredIonNameIsValid = ionTable.Lookup(IonNameTextbox.Text).isValid;
            IonLoadButton.IsEnabled = userEnteredIonNameIsValid;

            Ion ion = GetIonFromUserValues();
            if (ion != null && ion.isValid)
            {
                // modify gui to indicate ion is valid
                IonMobilityTextbox.Text = Math.Round(ion.mu * 1e-11, 4).ToString();
                IonAddButton.IsEnabled = true;
                ValidateIonSet();
            }
            else
            {
                IonAddButton.IsEnabled = false;
                IonMobilityTextbox.Text = "";
            }
        }

        #endregion

        #region ion set

        private void ValidateIonSet(object sender, RoutedEventArgs e)
        {
            ValidateIonSet();
        }

        private void AddIon(object sender, RoutedEventArgs e)
        {
            Ion ion = GetIonFromUserValues();
            ionSet.Add(ion);
            dataGrid1.Items.Refresh();
            ValidateIonSet();
        }

        private void ValidateIonSet(object sender, SelectionChangedEventArgs e) { ValidateIonSet(); }

        private void ValidateIonSet(object sender, EventArgs e) { ValidateIonSet(); }

        private void ValidateIonSet(object sender, TextChangedEventArgs e) { ValidateIonSet(); }

        private void ValidateIonSet()
        {
            if (CalculateButton is null)
                return;

            CalculateButton.IsEnabled = false;

            SaveButton.IsEnabled = (ionSet.Count > 0);

            MoveUpButton.IsEnabled = false;
            MoveDownButton.IsEnabled = false;
            RemoveButton.IsEnabled = false;

            int selectedIndex = dataGrid1.SelectedIndex;
            if (selectedIndex >= 0)
            {
                RemoveButton.IsEnabled = true;
                if (selectedIndex > 0)
                    MoveUpButton.IsEnabled = true;
                if (selectedIndex < dataGrid1.Items.Count - 1)
                    MoveDownButton.IsEnabled = true;
            }

            if (ionSet.Count < 2)
            {
                Message("Invalid Ion Set", "LJP calculation requires at least 2 ions");
                return;
            }

            bool containsAnions = false;
            bool containsCations = false;

            foreach (Ion ion in ionSet)
            {
                if (ion.charge > 0)
                    containsCations = true;
                if (ion.charge < 0)
                    containsAnions = true;
            }

            if (containsAnions == false || containsCations == false)
            {
                Message("Invalid Ion Set", "Ion sets must contain cations and anions");
                return;
            }

            Ion secondToLastIon = ionSet[ionSet.Count - 2];
            if (secondToLastIon.c0 == secondToLastIon.cL)
            {
                Message("Invalid Ion Set", "Second-to-last ion cannot have identical C0 and CL");
                return;
            }

            try
            {
                double tempC = double.Parse(CalculationTemperatureC.Text);
            }
            catch
            {
                Message("Invalid Temperature", "Enter a valid temperature");
                return;
            }

            Message("LJP Ready to Calculate", "");
            CalculateButton.IsEnabled = true;
        }

        private void IonSetSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool isItemSelected = (dataGrid1.SelectedIndex >= 0);
            bool lastItemSelected = (dataGrid1.SelectedIndex == dataGrid1.Items.Count - 1);
            bool firstItemSelected = (dataGrid1.SelectedIndex == 0);

            MoveUpButton.IsEnabled = isItemSelected && !firstItemSelected;
            MoveDownButton.IsEnabled = isItemSelected && !lastItemSelected;
            RemoveButton.IsEnabled = isItemSelected;

            SaveButton.IsEnabled = (ionSet.Count > 0);
        }


        private void IonSetRemoveButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedIndex = dataGrid1.SelectedIndex;
            ionSet.RemoveAt(dataGrid1.SelectedIndex);
            dataGrid1.Items.Refresh();
            if (ionSet.Count > 0)
                dataGrid1.SelectedIndex = Math.Min(selectedIndex, ionSet.Count - 1);
            dataGrid1.Items.Refresh();
            ValidateIonSet();
        }

        private void IonSetMoveUpButton_Click(object sender, RoutedEventArgs e)
        {
            int indexCut = dataGrid1.SelectedIndex;
            int indexPaste = indexCut - 1;

            Ion ion = ionSet[indexCut];
            ionSet.RemoveAt(indexCut);
            ionSet.Insert(indexPaste, ion);

            dataGrid1.Items.Refresh();
            ValidateIonSet();
        }

        private void IonSetMoveDownButton_Click(object sender, RoutedEventArgs e)
        {
            int indexCut = dataGrid1.SelectedIndex;
            int indexPaste = indexCut + 1;

            Ion ion = ionSet[indexCut];
            ionSet.RemoveAt(indexCut);
            ionSet.Insert(indexPaste, ion);

            dataGrid1.Items.Refresh();
            ValidateIonSet();
        }

        private void IonSetSaveButton_Click(object sender, RoutedEventArgs e)
        {
            Message("ERROR", "ion set save not yet implimented");
        }

        private void IonSetLoadButton_Click(object sender, RoutedEventArgs e)
        {
            Message("ERROR", "ion set load not yet implimented");
        }

        #endregion

        #region example ion sets

        private void LoadExampleClick(object sender, RoutedEventArgs e)
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
            dataGrid1.Items.Refresh();
            ValidateIonSet();
            CalculateLjp(null, null);
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
            dataGrid1.Items.Refresh();
            ValidateIonSet();
            CalculateLjp(null, null);
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
            dataGrid1.Items.Refresh();
            ValidateIonSet();
            CalculateLjp(null, null);
        }

        #endregion

        public event EventHandler AboutButtonClicked = delegate { };
        private void OnAboutButtonClicked(object sender, RoutedEventArgs e)
        {
            AboutButtonClicked(null, EventArgs.Empty);
        }

        private void CalculateLjp(object sender, RoutedEventArgs e)
        {
            double tempC = double.Parse(CalculationTemperatureC.Text);

            List<Ion> ionSetCopy = new List<Ion>();
            foreach (Ion ion in ionSet)
                ionSetCopy.Add(new Ion(ion));

            Stopwatch stopwatch = Stopwatch.StartNew();
            Message($"Calculating...", "");
            double ljp_V = Calculate.Ljp(ionSetCopy, tempC);
            double elapsedSec = (double)stopwatch.ElapsedTicks / Stopwatch.Frequency;

            StringBuilder msg = new StringBuilder();
            msg.AppendLine(string.Format("Equations were solved in {0:0.00} ms", elapsedSec * 1000.0));
            foreach (Ion ion in ionSetCopy)
                msg.AppendLine(ion.ToString());
            Message($"LJP = {ljp_V * 1000:0.000} mV", msg.ToString());

        }
    }
}
