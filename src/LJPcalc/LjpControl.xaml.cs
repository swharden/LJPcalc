using LJPmath;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
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
            ResetGui();
            //LoadExample_JLJP(null, null);

            if (ionTable != null)
            {
                var knownSets = new KnownIonSets(ionTable);
                foreach (var knownSet in knownSets.ionSets)
                {
                    MenuItem item = new MenuItem();
                    item.Click += ExampleIonSetClicked;
                    item.Header = knownSet.name;
                    item.ToolTip = knownSet.details;
                    KnownSetsContextMenu.Items.Add(item);
                }
            }
        }

        private void ExampleIonSetClicked(object sender, RoutedEventArgs e)
        {
            string clickedItemName = (((MenuItem)sender).Header).ToString();
            var knownSets = new KnownIonSets(ionTable);

            ionSet.Clear();
            foreach (var knownSet in knownSets.ionSets)
            {
                if (knownSet.name == clickedItemName)
                {
                    ionSet.AddRange(knownSet.ions);
                    CalculationTemperatureC.Text = knownSet.temperatureC.ToString();
                    ValidateIonSet();
                }
            }
            dataGrid1.Items.Refresh();
        }

        private void ResetGui()
        {
            IonNameTextbox.Text = "";
            IonChargeTextbox.Text = "";
            IonC0Textbox.Text = "";
            IonCLTextbox.Text = "";
            IonConductivityTextbox.Text = "";
            IonMobilityTextbox.Text = "";
            ValidateIonSet();
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
            }
            catch
            {
                IonTableListbox.Items.Add("ERROR");
                IonTableListbox.IsEnabled = false;
                MessageBox.Show(
                    "The ion table failed to load. Calculations will work, but ion values cannot be looked up and example ion sets will not be available.",
                    "Ion Table Error",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void IonTableListboxSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (IonTableListbox is null || ionTable is null || IonTableListbox.SelectedIndex < 0)
                return;

            string selectedIonName = IonTableListbox.SelectedItem.ToString();
            Ion ion = ionTable.Lookup(selectedIonName);

            IonNameTextbox.Text = ion.name;
            IonChargeTextbox.Text = ion.charge.ToString();
            IonConductivityTextbox.Text = (ion.conductivity * 1e4).ToString("0.000");
            IonC0Textbox.Text = "0";
            IonCLTextbox.Text = "0";
            Message($"Loaded Ion: {ion.nameWithCharge}", ion.ToString());

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

        private void IonNameKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Ion ion = ionTable.Lookup(IonNameTextbox.Text);
                if (ion.isValid)
                {
                    IonNameTextbox.Text = ion.name;
                    AddIon(null, null);
                    IonNameTextbox.Text = "";
                }
            }
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
                if (charge == 0)
                {
                    Message("No Charge", $"This molecule has no charge, so it does not influence LJP." +
                        Environment.NewLine + Environment.NewLine +
                        "Molecules without charge do not need to be added to the ion table."
                     );
                    return null;
                }

                double conductivity = double.Parse(IonConductivityTextbox.Text) * 1e-4;
                if (conductivity <= 0)
                {
                    Message("Low Conductivity", "This ion has low conductivity so its effect on LJP is negligible." +
                        Environment.NewLine + Environment.NewLine +
                        "Ions with low conductivity do not need to be added to the ion table."
                        );
                    return null;
                }

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
                Message("Add more ions...", "LJP calculation requires at least 2 ions");
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
            var dlg = new Microsoft.Win32.SaveFileDialog
            {
                Title = "Save Ion Set",
                FileName = "myIonSet",
                DefaultExt = ".md",
                Filter = "Markdown Files (.md)|*.md"
            };

            var result = dlg.ShowDialog();

            if (result == true)
            {
                string filePath = System.IO.Path.GetFullPath(dlg.FileName);
                var thisSet = new IonSet(ionSet);
                thisSet.Save(filePath);
                Message("Saved Ion Set", $"Custom ion set containing {thisSet.ions.Count} ions saved in:\n{filePath}");
            }
        }

        private void IonSetLoadButton_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Load Ion Set",
                FileName = "myIonSet",
                DefaultExt = ".md",
                Filter = "Markdown Files (.md)|*.md"
            };

            var result = dlg.ShowDialog();
            if (result == true)
            {
                string filePath = System.IO.Path.GetFullPath(dlg.FileName);
                var loadedIonSet = new IonSet(filePath);
                ionSet.Clear();
                ionSet.AddRange(loadedIonSet.ions);
                Message("Loaded Ion Set", $"Loaded {ionSet.Count} ions from file:\n{filePath}");
                dataGrid1.Items.Refresh();
                ValidateIonSet();
            }
        }

        #endregion

        #region example ion sets

        private void LoadExampleClick(object sender, RoutedEventArgs e)
        {
            ExampleButton.ContextMenu.IsOpen = true;
        }

        #endregion

        public event EventHandler AboutButtonClicked = delegate { };
        private void OnAboutButtonClicked(object sender, RoutedEventArgs e)
        {
            AboutButtonClicked(null, EventArgs.Empty);
        }

        private void ShowCalculationError(List<Ion> ionSetCopy)
        {
            if (AutoSortCheckbox.IsChecked.Value == true)
            {
                Message($"Ion Set Error", "This ion set failed to calculate," + Environment.NewLine +
                    "even though automatic ion set sorting was used." + Environment.NewLine +
                    "Please help improve LJPcalc by submitting a bug report!" + Environment.NewLine +
                    "Please include the full content of this message." + Environment.NewLine +
                    new IonSet(ionSetCopy).GetTableString()
                    );
            }
            else
            {
                Message($"Calculation Failed", "This ion set failed to calculate." + Environment.NewLine +
                    Environment.NewLine + "Enable automatic ion set sorting and try again.");
            }
        }

        public void CalculateLjpThread(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();

            double tempC = double.Parse(CalculationTemperatureC.Text);

            List<Ion> ionSetCopy = new List<Ion>();
            foreach (Ion ion in ionSet)
                ionSetCopy.Add(new Ion(ion));

            try
            {
                var ljp = Calculate.Ljp(ionSetCopy, tempC, AutoSortCheckbox.IsChecked.Value);
                if (ljp.isValid)
                    Message($"LJP = {ljp.mV:0.000} mV", ljp.report);
                else
                    ShowCalculationError(ionSetCopy);
            }
            catch (OperationCanceledException)
            {
                ShowCalculationError(ionSetCopy);
            }

            CalculateButton.IsEnabled = true;
        }

        private void CalculateLjp(object sender, RoutedEventArgs e)
        {
            Message($"Calculating...", "");
            CalculateButton.IsEnabled = false;

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.1);
            timer.Tick += CalculateLjpThread;
            timer.Start();
        }

        private void OnEditTableClicked(object sender, RoutedEventArgs e)
        {
            var res = MessageBox.Show($"LJPcalc loads ion valence and mobility from IonTable.md" +
                "\n\nDo you want to open this file now?", "Edit LJPcalc Ion Table",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Information);

            if (res == MessageBoxResult.Yes)
                System.Diagnostics.Process.Start("explorer.exe", ionTable.filePath);

            MessageBox.Show("IonTable.md will be reloaded the next time LJPcalc starts.",
                "Restart Required", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
