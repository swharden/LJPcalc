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
    /// Interaction logic for AddIonControl.xaml
    /// </summary>
    public partial class AddIonControl : UserControl
    {
        public AddIonControl()
        {
            InitializeComponent();
            LoadIonTable();
        }

        private void LoadIonTable()
        {
            ionTableListbox.Items.Clear();

            try
            {
                var ionTable = new LJPmath.IonTable();
                Debug.WriteLine(ionTable);
                foreach (var ion in ionTable.ions)
                    ionTableListbox.Items.Add(ion.name);
            }
            catch
            {
                ionTableListbox.Items.Add("ERROR");
                ionTableListbox.IsEnabled = false;
            }
        }
    }
}
