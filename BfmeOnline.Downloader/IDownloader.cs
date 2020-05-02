using System;
using System.Collections.Generic;
using System.Text;

namespace BfmeOnline.Downloader
{
    public interface IDownloader
    {
        void Cancel();

        void Download(string sourceUrl, string destinationPath);

    }
}
