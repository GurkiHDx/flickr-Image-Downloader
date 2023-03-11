using System.Net;
using flickr_Image_Downloader.Interfaces;

namespace flickr_Image_Downloader.Service;

public class Downloader : IDownloader
{
    private readonly string _filteredUrl;
    private readonly string _fileName;

    public Downloader(string filteredUrl, string fileName)
    {
        _filteredUrl = filteredUrl;
        _fileName = fileName;
    }

    public void Download()
    {
        Directory.CreateDirectory(@".\Download\");

        using (var client = new WebClient())
        {
            client.DownloadFile(_filteredUrl, @".\Download\" + _fileName);
        }
    }

}