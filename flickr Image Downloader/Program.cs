using flickr_Image_Downloader;
using flickr_Image_Downloader.Interfaces;
using flickr_Image_Downloader.Service;

class Program
{
    static void Main(string[] args)
    {
        // var url = "https://www.flickr.com/photos/dmtoh/";

        Console.WriteLine("-= flickr Image Downloader by GurkiHDx =-");

        do
        {
            Console.WriteLine("Input flickr url: ");
            var inputUrl = Console.ReadLine();

            if (inputUrl.Contains("https://www.flickr.com/photos/") ||
                inputUrl.Contains("http://www.flickr.com/photos/"))
            {
                Console.WriteLine("Download from: " + inputUrl);

                UrlChecker urlChecker = new UrlChecker();
                var checkedInputUrl = urlChecker.CheckInputUrl(inputUrl);

                IFlickrResponse flickrResponse = new FlickrResponse(inputUrl);
                var imageUrls = flickrResponse.GetImageUrls();

                foreach (var imageUrl in imageUrls)
                {
                    FlickrImage flickrImage = new FlickrImage(imageUrl);
                    flickrImage.AppendedUrl = urlChecker.BuildImageUrl(checkedInputUrl, imageUrl);

                    Download(flickrImage, flickrResponse);

                    Console.WriteLine("Download from {0} completed!", flickrImage.FileName);
                }

                Console.WriteLine("Download finished!");
                Console.WriteLine("Next url?");
            }
            else if (inputUrl.Equals("exit") || inputUrl.Equals("Exit"))
            {
                return;
            }
            else
            {
                Console.WriteLine("Wrong input. The Url should be start with 'https://www.flickr.com/photos/'");
            }

        } while (true);
    }

    private static void Download(FlickrImage flickrImage, IFlickrResponse flickrResponse)
    {
        flickrImage.HtmlResponse = flickrResponse.GetHtmlResponse(flickrImage.AppendedUrl);
        flickrImage.ImageSizePathes = flickrResponse.GetSizePathFromHtmlResponse(flickrImage.HtmlResponse).ToList();

        var imageHelper = new ImageHelper(flickrImage.ImageSizePathes);
        flickrImage.ImagePair = imageHelper.GetImagePair();

        var lastImageSize = flickrImage.ImagePair.Key;
        var lastImageUrl = "http://flickr.com" + flickrImage.ImagePair.Value;

        var lastImageResponse = flickrResponse.GetHtmlResponse(lastImageUrl);

        var imgPathFromHtmlResponse = flickrResponse.GetImgPathFromHtmlResponse(lastImageResponse);


        flickrImage.FilteredUrl = flickrResponse.GetFilteredUrl(imgPathFromHtmlResponse);

        flickrImage.FileName = flickrImage.GetFileName();

        // Console.WriteLine("Quality: {0} Url: {1}", lastImageSize, lastImageUrl);

        IDownloader dl = new Downloader(flickrImage.FilteredUrl, flickrImage.FileName);
        dl.Download();
    }
}