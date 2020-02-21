using System;
using System.Collections.Generic;
using System.Linq;
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

namespace LJPcalc
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Version version = typeof(AboutControl).Assembly.GetName().Version;
            Title = $"LJPcalc {version.Major}.{version.Minor}";

            ljpControl1.AboutButtonClicked += AboutShow;
        }

        private void AboutShow(object sender, EventArgs e)
        {
            aboutControl1.Visibility = Visibility.Visible;
            ljpControl1.Visibility = Visibility.Collapsed;
        }

        private void AboutHide(object sender, MouseButtonEventArgs e)
        {
            aboutControl1.Visibility = Visibility.Collapsed;
            ljpControl1.Visibility = Visibility.Visible;
        }
    }
}
