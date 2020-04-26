using System;
using System.Threading.Tasks;

namespace BfmeOnline.GameInstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            //Task.Run(() =>
            //{
            //    Installer.ExtractFiles(@"shared.zip", @".");
            //});

            //while (!Installer.Finished)
            //{
            //    Console.WriteLine("Progress " + Installer.Progress);
            //}

            //Task.Run(() =>
            //{
            //    Installer.Install("http://bfmedownload:8080/download-game");
            //});

            //while (Installer.State != InstallerState.FINISHED)
            //{
            //    Console.Write($"\r{Installer.State} Progress = {Installer.Progress}");
            //    Console.Write($"                                                                                         ");
                
            //}

            Installer.InstallRegistryKeys("E:\\bfme-game\\");
        }
    }
}
