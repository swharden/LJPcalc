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
    public partial class AddIonControl : UserControl
    {
        IonTable ionTable;

        public AddIonControl()
        {
            InitializeComponent();
            LoadIonTable();
            C0Textbox.Text = "0.0";
            CLTextbox.Text = "0.0";
        }

        private void LoadIonTable()
        {
            ionTableListbox.Items.Clear();

            try
            {
                ionTable = new IonTable();
                foreach (var ion in ionTable.ions)
                    ionTableListbox.Items.Add(ion.nameWithCharge);

                //ionTableGroupBox.Header = $"Ion Table ({ionTableListbox.Items.Count})";

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

        private void IonTableListbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ion ion = ionTable.ions[ionTableListbox.SelectedIndex];
            NameTextbox.Text = ion.name.ToString();
            ChargeTextbox.Text = ion.charge.ToString();
            ConductivityTextbox.Text = ion.conductance.ToString("E");
            MuTextbox.Text = ion.mu.ToString("E");
        }

        Ion GetIonFromTextbox()
        {
            try
            {
                Ion ion = new Ion(
                    name: NameTextbox.Text,
                    charge: int.Parse(ChargeTextbox.Text),
                    conductance: double.Parse(ConductivityTextbox.Text),
                    c0: double.Parse(C0Textbox.Text),
                    cL: double.Parse(CLTextbox.Text)
                    );
                return ion;
            }
            catch
            {
                return null;
            }
        }

        private void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Ion ion = GetIonFromTextbox();
            if (ion is null)
            {
                if (MuTextbox != null)
                    MuTextbox.Text = "";
                if (AddIonButtn != null)
                    AddIonButtn.IsEnabled = false;
            }
            else
            {
                if (MuTextbox != null)
                    MuTextbox.Text = ion.mu.ToString("E");
                if (AddIonButtn != null)
                    AddIonButtn.IsEnabled = true;
            }
        }

        public event EventHandler IonAdded = delegate { };

        private void AddIonButtn_Click(object sender, RoutedEventArgs e)
        {
            Ion ion = GetIonFromTextbox();
            IonAdded(ion, EventArgs.Empty);
        }

        private void ionTableListbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            AddIonButtn_Click(null, null);
        }
    }
}
