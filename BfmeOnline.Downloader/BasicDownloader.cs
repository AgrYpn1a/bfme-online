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
        private const int THREADS = 4;

        private int[] _progress = new int[THREADS];

        public bool IsFinished { get; private set; }

        private volatile bool _isCancelled = false;

        public Action<Exception> OnError;
        public Action OnDownloadFinished;

        public int FileSizeMB = 0;

        private object _root = new object();

        private float CalculateProgress()
        {
            int progress = 0;
            foreach (var p in _progress)
            {
                progress += p;
            }

            return progress / (float)THREADS;
        }

        private Task[] _tasks = new Task[THREADS];
        private string[] _fileNames = new string[THREADS];

        public BasicDownloader(string sourceUrl, string downloadPath) : base(sourceUrl, downloadPath)
        {
            Console.WriteLine($"Source = {_sourceUrl}");
            Console.WriteLine($"Download = {_downloadPath}");
        }

        public async override Task Download()
        {
            lock (_root)
            {
                IsFinished = false;
            }

            // Clear old files
            ClearDownloadFiles(_downloadPath);

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
                        float progress = CalculateProgress();
                        OnProgressUpdate?.Invoke((int)progress);
                    }
                });
                progress.Start();

                // Create threads
                for (int i = 0; i < THREADS; i++)
                {
                    int chunkIndex = i;

                    _tasks[i] = new Task(() =>
                    {
                        // Save file names
                        string fileName = GetChunkName(chunkIndex);
                        _fileNames[chunkIndex] = fileName;

                        Console.WriteLine($"Starting Thread... ChunkIndex = {chunkIndex}, fileName = {fileName}");

                        int offset = (chunkIndex > 0) ? 1 : 0;

                        DownloadChunk(fileName, chunkIndex, chunkIndex * fileChunkSize + offset, (chunkIndex + 1) * fileChunkSize);
                    });
                }

                foreach (var task in _tasks)
                {
                    task.Start();
                }

                await Task.WhenAll(_tasks);

                lock (_root)
                {
                    MergeTempFiles();
                    IsFinished = true;
                }
            }
            catch (Exception e)
            {
                // Throw further
                throw e;
            }
        }

        public override void Cancel()
        {
            lock (_root)
            {
                _isCancelled = true;
                IsFinished = true;
            }

            ClearTempFiles();
            ClearDownloadFiles(_downloadPath);
        }

        private void MergeTempFiles()
        {
            if (File.Exists(_downloadPath))
            {
                return;
            }

            using (FileStream destinationStream = new FileStream(_downloadPath, FileMode.Append))
            {
                foreach (var fileName in _fileNames)
                {
                    try
                    {
                        byte[] file = File.ReadAllBytes(fileName);
                        destinationStream.Write(file, 0, file.Length);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }

                // Make sure stream is closed after writing
                destinationStream.Close();

                ClearTempFiles();
            }
        }

        private string GetChunkName(int chunkId) => $"{_downloadPath}_tmp_{chunkId}";

        private void ClearTempFiles()
        {
            //for(int i=0; i<THREADS; i++)
            //{
            //    string fileName = GetChunkName(i);
            //    File.Delete(fileName);
            //}

            foreach (var fileName in _fileNames)
            {
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

            Console.WriteLine($"FileSize = {hwRes.ContentLength}");

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
                File.Delete(filePath);
                //saveFileStream = new System.IO.FileStream(filePath,
                //  System.IO.FileMode.Append, System.IO.FileAccess.Write,
                //  System.IO.FileShare.ReadWrite);
            }

            // Create directory
            string[] splitPath = filePath.Split(Path.DirectorySeparatorChar);
            Directory.CreateDirectory(Path.Combine(splitPath.SkipLast(1).ToArray()));

            saveFileStream = new System.IO.FileStream(filePath,
                System.IO.FileMode.Create, System.IO.FileAccess.Write,
                System.IO.FileShare.ReadWrite);

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

    }

    class Program
    {
        static void Main(string[] args)
        {
            ADownloader downloader = new BasicDownloader("http://admin-api.thebfmeonline.com/api/patch/download", @"c:\temp\patch.zip");
            //ADownloader downloader = new BasicDownloader("http://localhost:3000/api/patch/download", @"c:\temp\patch.zip");
            downloader.OnProgressUpdate = (progress) =>
            {
                Console.WriteLine($"Progress {progress}");
            };

            Thread t = new Thread(async () =>
            {
                try
                {
                    await downloader.Download();
                    //await ((BasicDownloader)downloader).MergeTempFiles();
                    Console.WriteLine("Download finished.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            });

            t.Start();
            t.Join();
        }
    }
}
