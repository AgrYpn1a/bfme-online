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
    }

}
