using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace BfmeOnline.Downloader
{

    public class Downloader
    {
        private string _sourceUrl;
        private string _destinationPath;
        private const int THREADS = 4;

        private int[] _progress = new int[THREADS];

        private volatile bool _isFinished = false;
        public bool IsFinished => _isFinished;

        public Action<int> OnProgressUpdate;
        public Action OnDownloadFinished;

        private int CalculateProgress()
        {
            int progress = 0;
            foreach (var p in _progress)
            {
                progress += p;
            }

            return progress / THREADS;
        }

        public void DownloadFile(string sourceUrl, string destinationPath)
        {
            _isFinished = false;
            _sourceUrl = sourceUrl;
            _destinationPath = destinationPath;

            long fileChunkSize = GetFileSize() / (long)THREADS;

            Thread[] dlThreads = new Thread[THREADS];
            string[] fileNames = new string[THREADS];

            // Start progress report thread
            new Thread(() =>
            {
                while (!IsFinished)
                {
                    Thread.Sleep(100);
                    int progress = CalculateProgress();
                    OnProgressUpdate?.Invoke(progress);

                    // TODO remove debug
                    Console.WriteLine(progress);
                }
            }).Start();

            // Create threads
            for (int i = 0; i < THREADS; i++)
            {
                int index = i;

                Thread t = new Thread(() =>
                {
                    // Save file names
                    string fileName = $"{_destinationPath}_tmp_{index}";
                    fileNames[index] = fileName;

                    // Initiate download
                    Console.WriteLine($"Thread {index} started.");

                    int offset = (index > 0) ? 1 : 0;

                    DownloadChunk(fileName, index, index * fileChunkSize + offset, (index + 1) * fileChunkSize);
                });

                t.Start();

                dlThreads[i] = t;
            }

            foreach (var thread in dlThreads)
            {
                thread.Join();
            }

            // Merge files
            using (FileStream destinationStream = new FileStream(destinationPath, FileMode.Append))
            {
                foreach (var fileName in fileNames)
                {
                    byte[] file = File.ReadAllBytes(fileName);
                    destinationStream.Write(file, 0, file.Length);
                }
            }

            _isFinished = true;
            OnDownloadFinished?.Invoke();
        }

        private long GetFileSize()
        {
            HttpWebRequest hwRq = (HttpWebRequest)HttpWebRequest.Create(_sourceUrl);
            HttpWebResponse hwRes = (HttpWebResponse)hwRq.GetResponse();

            Stream smRespStream = hwRes.GetResponseStream();

            return hwRes.ContentLength;
        }

        private void DownloadChunk(string filePath, int index, long from, long to)
        {
            long fileSize = 0;
            int bufferSize = 1024 * 1000;
            long existLen = 0;

            System.IO.FileStream saveFileStream;

            if (System.IO.File.Exists(filePath))
            {
                System.IO.FileInfo fINfo =
                   new System.IO.FileInfo(filePath);
                existLen = fINfo.Length;
            }

            if (existLen > 0)
            {
                saveFileStream = new System.IO.FileStream(filePath,
                  System.IO.FileMode.Append, System.IO.FileAccess.Write,
                  System.IO.FileShare.ReadWrite);
            }
            else
            {
                saveFileStream = new System.IO.FileStream(filePath,
                  System.IO.FileMode.Create, System.IO.FileAccess.Write,
                  System.IO.FileShare.ReadWrite);
            }

            System.Net.HttpWebRequest hwRq = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(_sourceUrl);
            hwRq.AddRange(from, to);

            System.Net.HttpWebResponse hwRes;

            System.IO.Stream smRespStream;
            hwRes = (System.Net.HttpWebResponse)hwRq.GetResponse();
            smRespStream = hwRes.GetResponseStream();

            fileSize = hwRes.ContentLength;

            int byteSize;
            long totalByteSize = 0;
            byte[] downBuffer = new byte[bufferSize];

            while ((byteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
            {
                totalByteSize += byteSize;
                saveFileStream.Write(downBuffer, 0, byteSize);

                int progress = (int)((float)totalByteSize / fileSize * 100);
                _progress[index] = progress;
            }

            saveFileStream.Close();
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            Downloader downloader = new Downloader();
            downloader.DownloadFile("https://speed.hetzner.de/100MB.bin", @"e:\temp\100mb.bin");
        }
    }
}
