using BfmeOnline.Launcher.Source.patcher.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace BfmeOnline.Launcher.Source.patcher
{
    internal class Patcher
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

        public static Dictionary<string, string> GetHashSumsDict(string rootDir) => GetHashSums(rootDir).ToDictionary(fileHash => fileHash.Path, fileHash => fileHash.Hash);
    }
}
