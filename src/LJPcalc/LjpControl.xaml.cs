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
    /// Interaction logic for MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();
            addIonControl1.IonAdded += OnAddIon;
            ionSetControl1.IonSetChanged += OnIonSetChanged;

            ionSetControl1.AddIon(new Ion());
            ionSetControl1.ClearIons();
        }

        private void OnAddIon(object sender, EventArgs e)
        {
            Ion ion = (Ion)sender;
            ionSetControl1.AddIon(ion);
        }

        private void OnIonSetChanged(object sender, EventArgs e)
        {
            List<Ion> ionSet = (List<Ion>)sender;
            calculationControl1.Calculate(ionSet);
        }
    }
}
