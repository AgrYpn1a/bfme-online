using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;


namespace BfmeOnline.OptionsEditor
{
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

        public static OptionsINI ParseFromFile()
        {
            OptionsINI optionsIni = new OptionsINI();

            using (StreamReader sr = new StreamReader(PATH_OPTIONS_INI))
            {
                while (!sr.EndOfStream)
                {
                    string[] lines = sr.ReadLine().Split('=');

                    string key = lines[0].Trim();
                    string value = lines[1].Trim();

                    if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.ShowAllHealthBars)).StringName.Equals(key))
                    {
                        optionsIni.ShowAllHealthBars = OptionsINI.GetEnumValue<YesNoOption>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.ShowUnitDecals)).StringName.Equals(key))
                    {
                        optionsIni.ShowUnitDecals = OptionsINI.GetEnumValue<YesNoOption>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.AltMouseSetup)).StringName.Equals(key))
                    {
                        optionsIni.AltMouseSetup = OptionsINI.GetEnumValue<YesNoOption>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.Resolution)).StringName.Equals(key))
                    {
                        optionsIni.Resolution = OptionsINI.GetEnumValue<Resolution>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.Brightness)).StringName.Equals(key))
                    {
                        optionsIni.Brightness = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.ScrollSpeed)).StringName.Equals(key))
                    {
                        optionsIni.ScrollSpeed = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.AudioLOD)).StringName.Equals(key))
                    {
                        optionsIni.AudioLOD = OptionsINI.GetEnumValue<AudioLOD>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.GeneralDetails)).StringName.Equals(key))
                    {
                        optionsIni.GeneralDetails = OptionsINI.GetEnumValue<Details>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.VolAmbient)).StringName.Equals(key))
                    {
                        optionsIni.VolAmbient = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.VolVoice)).StringName.Equals(key))
                    {
                        optionsIni.VolVoice = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.VolMusic)).StringName.Equals(key))
                    {
                        optionsIni.VolMusic = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.VolSFX)).StringName.Equals(key))
                    {
                        optionsIni.VolSFX = int.Parse(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.UseEAX)).StringName.Equals(key))
                    {
                        optionsIni.UseEAX = OptionsINI.GetEnumValue<YesNoOption>(value);
                    }
                    else if (OptionsINI.GetAttribute<Attributes.OptionNameAttribute>(nameof(optionsIni.FPSLimit)).StringName.Equals(key))
                    {
                        optionsIni.FPSLimit = OptionsINI.GetEnumValue<YesNoOption>(value);
                    }
                    else
                    {
                        //throw new Exception("Uknown option.");
                    }
                }
            }

            return optionsIni;
        }
    }

}
