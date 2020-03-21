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
        public Options()
        {
            InitializeComponent();
            DataContext = this;
            PopulateComboboxFromEnum();
        }

        private void PopulateComboboxFromEnum()
        {
            cbResoultion.ItemsSource = Enum.GetValues(typeof(Resolution)).Cast<Resolution>().ToList()?.Select(s => s.ToDescriptionString());
        }
    }
}
