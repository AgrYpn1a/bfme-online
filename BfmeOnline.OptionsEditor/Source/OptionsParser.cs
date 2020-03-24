using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;

/**
 * Options.ini for Bfme1
 * =======================
 
    AllHealthBars = yes
    AlternateMouseSetup = no
    AmbientVolume = 0
    AudioLOD = High
    Brightness = 50
    FirewallBehavior = 1
    FirewallNeedToRefresh = FALSE
    FirewallPortAllocationDelta = 0
    FirewallPortOverride = 16000
    FixedStaticGameLOD = UltraHigh
    FlashTutorial = 0
    HeatEffects = yes
    IdealStaticGameLOD = High
    IsThreadedLoad = yes
    MovieVolume = 0
    MusicVolume = 0
    Resolution = 1600 1200
    SFXVolume = 0
    ScrollFactor = 38
    StaticGameLOD = UltraHigh
    TimesInGame = 153
    UnitDecals = no
    UseEAX3 = no
    VoiceVolume = 0
*/

namespace BfmeOnline.OptionsEditor
{
    namespace Attributes
    {
        [AttributeUsage(AttributeTargets.Property)]
        public class OptionIntConstraintAttribute : Attribute
        {
            public int Min { get; private set; }
            public int Max { get; private set; }

            public OptionIntConstraintAttribute(int min, int max)
            {
                Min = min;
                Max = max;
            }
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class OptionName : Attribute
        {
            public string StringName { get; private set; }

            public OptionName(string name)
            {
                StringName = name;
            }
        }

        [AttributeUsage(AttributeTargets.Field)]
        public class OptionStringValue : Attribute
        {
            public string StringValue { get; private set; }

            public OptionStringValue(string value)
            {
                StringValue = value;
            }
        }
    }

    public sealed class OptionsINI
    {
        [Attributes.OptionName("AllHealthBars")]
        public YesNoOption ShowAllHealthBars { get; set; } = YesNoOption.YES;

        [Attributes.OptionName("UnitDecals")]
        public YesNoOption ShowUnitDecals { get; set; } = YesNoOption.NO;

        [Attributes.OptionName("FPSLimit")]
        public YesNoOption FPSLimit { get; set; } = YesNoOption.NO;

        [Attributes.OptionName("AlternateMouseSetup")]
        public YesNoOption AltMouseSetup { get; set; } = YesNoOption.NO;


        [Attributes.OptionName("Resolution ")]
        public Resolution Resolution { get; set; } = Resolution.res_1920x1080;

        [Attributes.OptionName("Brightness ")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int Brightness { get; set; } = 50;

        [Attributes.OptionName("ScrollFactor")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int ScrollSpeed { get; set; } = 50;

        // LOD
        [Attributes.OptionName("AudioLOD")]
        public AudioLOD AudioLOD { get; set; } = AudioLOD.HIGH;

        [Attributes.OptionName("StaticGameLOD")]
        public Details GeneralDetails { get; set; } = Details.ULTRA_HIGH;

        // Volume
        [Attributes.OptionName("AmbientVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolAmbient { get; set; } = 50;

        [Attributes.OptionName("VoiceVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolVoice { get; set; } = 50;

        [Attributes.OptionName("MovieVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolMovie { get; set; } = 50;

        [Attributes.OptionName("MusicVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolMusic { get; set; } = 50;

        [Attributes.OptionName("SFXVolume")]
        [Attributes.OptionIntConstraint(0, 100)]
        public int VolSFX { get; set; } = 50;

        [Attributes.OptionName("UseEAX3")]
        public YesNoOption UseEAX { get; set; } = YesNoOption.NO;

        public static T GetAttribute<T>(Object property) where T : Attribute
        {
            var a = property.GetType().GetCustomAttribute(typeof(T)) as T;
            return a;
        }

        public static T GetEnumValue<T>(string value) where T : Enum
        {
            return Enum.GetValues(typeof(T)).Cast<T>().ToList().Find(val => val.ToDescriptionString().Equals(value));
        }

        public Dictionary<string, string> GetOptionsDict()
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();

            dict.Add(GetAttribute<Attributes.OptionName>(ShowAllHealthBars).StringName, GetAttribute<Attributes.OptionStringValue>(ShowAllHealthBars).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(ShowUnitDecals).StringName, GetAttribute<Attributes.OptionStringValue>(ShowUnitDecals).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(AltMouseSetup).StringName, GetAttribute<Attributes.OptionStringValue>(AltMouseSetup).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(Resolution).StringName, GetAttribute<Attributes.OptionStringValue>(Resolution).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(Brightness).StringName, Brightness.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(ScrollSpeed).StringName, ScrollSpeed.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(AudioLOD).StringName, GetAttribute<Attributes.OptionStringValue>(AudioLOD).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(GeneralDetails).StringName, GetAttribute<Attributes.OptionStringValue>(GeneralDetails).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(GeneralDetails).StringName, GetAttribute<Attributes.OptionStringValue>(GeneralDetails).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(VolAmbient).StringName, VolAmbient.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(VolVoice).StringName, VolVoice.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(VolMusic).StringName, VolMusic.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(VolSFX).StringName, VolSFX.ToString());
            dict.Add(GetAttribute<Attributes.OptionName>(UseEAX).StringName, GetAttribute<Attributes.OptionStringValue>(UseEAX).StringValue);
            dict.Add(GetAttribute<Attributes.OptionName>(FPSLimit).StringName, GetAttribute<Attributes.OptionStringValue>(FPSLimit).StringValue);

            // Hardcoded options
            dict.Add("FirewallBehavior", "1");
            dict.Add("FirewallNeedToRefresh", "FALSE");
            dict.Add("FirewallPortAllocationDelta", "0");
            dict.Add("FirewallPortOverride", "16000");
            dict.Add("FlashTutorial", "0");
            dict.Add("IsThreadedLoad", "yes");
            dict.Add("IsThreadedLoad", "yes");

            return dict;
        }
    }

    public static class OptionsParser
    {
        public static OptionsINI GetDefaultConfig()
        {
            return new OptionsINI();
        }

        private static readonly string PATH_OPTIONS_INI =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "My Battle for Middle-earth Files", "Options.ini");

        /// <summary>
        /// Takes OptionsINI config object and writes to options.ini file.
        /// </summary>
        /// <param name="options"></param>
        public static void DumpOptionsToFile(OptionsINI optionsIni)
        {
            try
            {
                // Create a file to write to.
                using (StreamWriter sw = File.CreateText(PATH_OPTIONS_INI))
                {
                    var options = optionsIni.GetOptionsDict();
                    options.ToList().ForEach(keyValPair =>
                    {
                        sw.WriteLine($"{keyValPair.Key} = {keyValPair.Value}");
                    });
                }

                MessageBox.Show("Options saved successfully!");
            }
            catch (Exception e)
            {
                MessageBox.Show($"Error: {e}");
            }
        }
    }

}
