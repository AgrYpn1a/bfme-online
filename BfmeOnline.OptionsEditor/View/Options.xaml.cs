using BfmeOnline.OptionsEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BfmeOnlineLauncher.View
{
    /// <summary>
    /// Interaction logic for Options.xaml
    /// </summary>
    public partial class Options : Window
    {
        public string Resolution { get; set; }
        public Options()
        {
            InitializeComponent();
            DataContext = this;
            PopulateComboboxFromEnum();
            OptionsDefaultInit();
        }

        private void PopulateComboboxFromEnum()
        {
            cbResoultion.ItemsSource = Enum.GetValues(typeof(Resolution)).Cast<Resolution>().ToList()?.Select(s => s.ToDescriptionString());
            cbDetails.ItemsSource = Enum.GetValues(typeof(Details)).Cast<Details>().ToList()?.Select(s => s.ToDescriptionString());
        }

        private void OptionsDefaultInit()
        {
            var optionsIni = OptionsParser.GetDefaultConfig();

            Resolution = optionsIni.Resolution.ToDescriptionString();
        }

        private void btnAcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            OptionsParser.DumpOptionsToFile(
                new OptionsINI()
                {
                    Resolution = OptionsINI.GetEnumValue<Resolution>(this.Resolution)
                }
            );
            this.OnClosed();
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            OptionsDefaultInit();
            this.OnClosed();
        }

        private void OnClosed()
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.OnClosed();
        }
    }
}
