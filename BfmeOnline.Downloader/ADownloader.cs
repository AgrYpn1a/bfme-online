using System;
using System.Threading.Tasks;

namespace BfmeOnline.Downloader
{
    public abstract class ADownloader
    {
        // Events
        public Action<int> OnProgressUpdate { get; set; }

        protected string _sourceUrl;
        protected string _downloadPath;

        public ADownloader(string sourceUrl, string downloadPath)
        {
            _sourceUrl = sourceUrl;
            _downloadPath = downloadPath;
        }

        public abstract void Cancel();

        public abstract Task Download(string sourceUrl, string destinationPath);

    }
}
