using System;
using System.IO;
using System.Net;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BfmeOnline.Downloader
{

    public class BasicDownloader : ADownloader
    {
        private string _sourceUrl;
        private string _destPath;
        private const int THREADS = 4;

        private int[] _progress = new int[THREADS];

        public bool IsFinished { get; private set; }

        private volatile bool _isCancelled = false;

        public Action<Exception> OnError;
        public Action OnDownloadFinished;

        public int FileSizeMB = 0;

        private object _root = new object();

        private int CalculateProgress()
        {
            int progress = 0;
            foreach (var p in _progress)
            {
                progress += p;
            }

            return progress / THREADS;
        }

        private Task[] _tasks = new Task[THREADS];
        private string[] _fileNames = new string[THREADS];

        public BasicDownloader(string sourceUrl, string downloadPath) : base(sourceUrl, downloadPath)
        {
        }

        public async override Task Download(string url, string destPath)
        {
            lock (_root)
            {
                IsFinished = false;
            }

            _destPath = destPath;

            // Clear old files
            ClearDownloadFiles(destPath);
            ClearTempFiles(destPath);

            try
            {
                long fileChunkSize = GetFileSize() / (long)THREADS;
                _fileNames = new string[THREADS];

                // Start progress report thread
                Thread progress = new Thread(() =>
                {
                    while (!IsFinished)
                    {
                        Thread.Sleep(100);
                        int progress = CalculateProgress();
                        OnProgressUpdate?.Invoke(progress);
                    }
                });
                progress.Start();

                // Create threads
                for (int i = 0; i < THREADS; i++)
                {
                    int index = i;

                    _tasks[i] = new Task(() =>
                    {
                        // Save file names
                        string fileName = GetChunkName(destPath, i);
                        _fileNames[index] = fileName;

                        int offset = (index > 0) ? 1 : 0;

                        DownloadChunk(fileName, index, index * fileChunkSize + offset, (index + 1) * fileChunkSize);
                    });
                }

                foreach (var task in _tasks)
                {
                    task.Start();
                }

                await Task.WhenAll(_tasks);
                lock (_root)
                {
                    IsFinished = true;
                }
            }
            catch (Exception e)
            {
                // Throw further
                throw e;
            }
        }

        public void CancelDownload()
        {
            lock (_root)
            {
                _isCancelled = true;
                IsFinished = true;
            }

            ClearTempFiles(_destPath);
            ClearDownloadFiles(_destPath);
        }

        public async Task MergeTempFiles()
        {
            if (File.Exists(_destPath))
            {
                return;
            }

            using (FileStream destinationStream = new FileStream(_destPath, FileMode.Append))
            {
                foreach (var fileName in _fileNames)
                {
                    byte[] file = File.ReadAllBytes(fileName);
                    await destinationStream.WriteAsync(file, 0, file.Length);
                }

                // Make sure stream is closed after writing
                destinationStream.Close();

                // Clear temp files
                foreach (var fileName in _fileNames)
                {
                    File.Delete(fileName);
                }
            }
        }

        private string GetChunkName(string destPath, int chunkId) => $"{destPath}_tmp_{chunkId}";

        private void ClearTempFiles(string destPath)
        {
            for (int i = 0; i < THREADS; i++)
            {
                string fileName = GetChunkName(destPath, i);
                File.Delete(fileName);
            }
        }

        private void ClearDownloadFiles(string destPath)
        {
            File.Delete(destPath);
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

            try
            {
                while (!_isCancelled && (byteSize = smRespStream.Read(downBuffer, 0, downBuffer.Length)) > 0)
                {
                    totalByteSize += byteSize;
                    saveFileStream.Write(downBuffer, 0, byteSize);

                    int progress = (int)((float)totalByteSize / fileSize * 100);
                    _progress[index] = progress;
                }

                saveFileStream.Close();
            }
            catch (Exception e)
            {
                OnError?.Invoke(e);
            }

            // Cleanup
            if (_isCancelled)
            {
                File.Delete(filePath);
            }
        }

        public override void Cancel()
        {
        }


    }

    class Program
    {
        static void Main(string[] args)
        {
            ADownloader downloader = new BasicDownloader("", "");
            downloader.OnProgressUpdate = (progress) =>
            {
                Console.WriteLine($"Progress {progress}");
            };

            Thread t = new Thread(async () =>
            {
                await downloader.Download("https://speed.hetzner.de/100MB.bin", @"e:\temp\100mb.bin");
                Console.WriteLine("Download finished.");
            });

            t.Start();
        }
    }
}
