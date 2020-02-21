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
    /// Interaction logic for AboutControl.xaml
    /// </summary>
    public partial class AboutControl : UserControl
    {
        public AboutControl()
        {
            InitializeComponent();

            Version version = typeof(AboutControl).Assembly.GetName().Version;
            versionLabel.Content = $"{version.Major}.{version.Minor}";
        }

        public event EventHandler HideAboutScreen = delegate { };
        private void DockPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            HideAboutScreen(null, EventArgs.Empty);
        }

        private void UrlLaunch(string url)
        {
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

        private void Link_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UrlLaunch("https://github.com/swharden/LJPcalc");
        }

        private void urlScott_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UrlLaunch("https://www.swharden.com/wp/about-scott/");
        }

        private void urlDoriano_MouseUp(object sender, MouseButtonEventArgs e)
        {
            UrlLaunch("https://sites.google.com/site/dbrogioli/");
        }
    }
}
