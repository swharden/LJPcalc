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

        private void DemoButton_Click(object sender, RoutedEventArgs e)
        {
            ionSet.Clear();

            ionSet.Add(new Ion("Zn", 9, 0.0284));
            ionSet.Add(new Ion("K", 0, 3));
            ionSet.Add(new Ion("Cl", 18, 3.0568));

            var ionTable = new IonTable();
            for (int i = 0; i < ionSet.Count; i++)
                ionSet[i] = ionTable.Lookup(ionSet[i]);

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

        private void UpdateGuiFromIonSet()
        {
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
    }
}
