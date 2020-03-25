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
        public string Details { get; set; }
        public int Brightness { get; set; }
        public bool ShowAllHealthBars { get; set; }
        public bool ShowUnitDecals { get; set; }
        public bool FrameLimit { get; set; }
        public int ScrollSpeed { get; set; }
        public bool AlternateMouseSetup { get; set; }
        public int Music { get; set; }
        public int SoundFx { get; set; }
        public int Voice { get; set; }
        public int Ambient { get; set; }
        public bool Eax3 { get; set; }


        public Options()
        {
            InitializeComponent();
            DataContext = this;
            PopulateComboboxFromEnum();
            OptionsDefaultInit();
        }

        private void PopulateComboboxFromEnum()
        {
            cmbResoultion.ItemsSource = Enum.GetValues(typeof(Resolution)).Cast<Resolution>().ToList()?.Select(s => s.ToDescriptionString());
            cmbDetails.ItemsSource = Enum.GetValues(typeof(Details)).Cast<Details>().ToList()?.Select(s => s.ToDescriptionString());
        }

        private void OptionsDefaultInit()
        {
            //var optionsIni = OptionsParser.GetDefaultConfig();
            var optionsIni = OptionsParser.ParseFromFile();

            Resolution = optionsIni.Resolution.ToDescriptionString();
            Details = optionsIni.GeneralDetails.ToDescriptionString();
            Brightness = optionsIni.Brightness;
            ShowAllHealthBars = optionsIni.ShowAllHealthBars == YesNoOption.YES;
            ShowUnitDecals = optionsIni.ShowUnitDecals == YesNoOption.YES;
            FrameLimit = optionsIni.FPSLimit == YesNoOption.YES;
            ScrollSpeed = optionsIni.ScrollSpeed;
            AlternateMouseSetup = optionsIni.AltMouseSetup == YesNoOption.YES;
            Music = optionsIni.VolMusic;
            SoundFx = optionsIni.VolSFX;
            Voice = optionsIni.VolVoice;
            Ambient = optionsIni.VolAmbient;
            Eax3 = optionsIni.UseEAX == YesNoOption.YES;

        }

        private void btnAcceptChanges_Click(object sender, RoutedEventArgs e)
        {
            OptionsParser.DumpOptionsToFile(
                new OptionsINI()
                {
                    Resolution = OptionsINI.GetEnumValue<Resolution>(this.Resolution),
                    GeneralDetails = OptionsINI.GetEnumValue<Details>(this.Details),
                    Brightness = this.Brightness,
                    ShowAllHealthBars = this.ShowAllHealthBars ? YesNoOption.YES : YesNoOption.NO,
                    ShowUnitDecals = this.ShowUnitDecals ? YesNoOption.YES : YesNoOption.NO,
                    FPSLimit = this.FrameLimit ? YesNoOption.YES : YesNoOption.NO,
                    ScrollSpeed = this.ScrollSpeed,
                    AltMouseSetup = this.AlternateMouseSetup ? YesNoOption.YES : YesNoOption.NO,
                    VolMusic = this.Music,
                    VolSFX = this.SoundFx,
                    VolVoice = this.Voice,
                    VolAmbient = this.Ambient,
                    UseEAX = this.Eax3 ? YesNoOption.YES : YesNoOption.NO
                }
            );
            this.OnClosed();
        }

        private void btnResetSettings_Click(object sender, RoutedEventArgs e)
        {
            OptionsDefaultInit();

            //this.OnClosed();
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
