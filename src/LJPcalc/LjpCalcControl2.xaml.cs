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
    /// Interaction logic for LjpCalcControl.xaml
    /// </summary>
    public partial class LjpCalcControl : UserControl
    {
        IonTable ionTable; // TODO: verify this is not null when accessed

        public LjpCalcControl()
        {
            InitializeComponent();
            LoadIonTable();
        }

        private void LoadIonTable()
        {
            ionTableListbox.Items.Clear();

            try
            {
                ionTable = new IonTable();
                foreach (var ion in ionTable.ions)
                    ionTableListbox.Items.Add(ion.nameWithCharge);

                // pre-select an item
                ionTableListbox.SelectedItem = ionTableListbox.Items[0];
                ionTableListbox.ScrollIntoView(ionTableListbox.SelectedItem);
            }
            catch
            {
                ionTableListbox.Items.Add("ERROR");
                ionTableListbox.IsEnabled = false;
            }
        }

        private void TableIonSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void TableIonDoubleClicked(object sender, MouseButtonEventArgs e)
        {

        }

        private void AddIonNameChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddIonChargeChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddIonCondChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddIonC0Changed(object sender, TextChangedEventArgs e)
        {

        }

        private void AddIonCLChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void AddIon(object sender, RoutedEventArgs e)
        {

        }

        private void dataGridSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void dataGridCellChanged(object sender, EventArgs e)
        {

        }

        private void IonSetMoveUpButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetMoveDownButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetRemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetExampleButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetExampleJLJP(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetExampleNgAndBarry(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetExampleJPWin(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetClear(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetSaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void IonSetLoadButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
