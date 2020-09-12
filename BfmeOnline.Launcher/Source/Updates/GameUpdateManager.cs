using BfmeOnline.Launcher.Source.Model;
using BfmeOnline.Launcher.Source.patcher.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace BfmeOnline.Launcher.Source.Updates
{
    sealed class GameUpdateManager : IUpdateManager
    {

        private static string[] GetFilePaths(string rootDir)
        {
            try
            {
                return Directory.GetFiles(rootDir, "*", SearchOption.AllDirectories);
            }
            catch (Exception e)
            {
                return new[] { e.Message.ToString() };
            }
        }

        public static List<FileHash> GetHashSums(string rootDir)
        {
            List<FileHash> fileHashList = new List<FileHash>();
            foreach (string path in GetFilePaths(rootDir))
            {
                var md5 = MD5.Create();
                {
                    var stream = File.OpenRead(path);
                    {
                        fileHashList.Add(new FileHash
                        {
                            Path = path.Replace(rootDir, "").Substring(1),
                            Hash = BitConverter.ToString(md5.ComputeHash(stream)).Replace("-", "").ToLowerInvariant()
                        });
                    }
                }
            }

            return fileHashList;
        }

        public Task<bool> HasUpdates()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
