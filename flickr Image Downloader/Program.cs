using System.Text;
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

                IFlickrResponse flickrResponse = new FlickrResponse();

                var htmlResponse = flickrResponse.GetHtml(inputUrl);

                var imageUrls = flickrResponse.GetImgPathFromHtmlResponse(htmlResponse);

                UrlChecker urlChecker = new UrlChecker();
                var checkedInputUrl = urlChecker.CheckInputUrl(inputUrl);

                foreach (var imageUrl in imageUrls)
                {
                    var fileName = Download(urlChecker, imageUrl, checkedInputUrl, flickrResponse);

                    Console.WriteLine("Download from {0} completed!", fileName);
                }

                Console.WriteLine("Download finished!");
                Console.WriteLine("Next url?");
            }
            else if (inputUrl.Equals("exit") || inputUrl.Equals("Exit"))
            {
                break;
            }
            else
            {
                Console.WriteLine("Wrong input. The Url should be start with 'https://www.flickr.com/photos/'");
            }

        } while (true);
    }

    private static string Download(
        UrlChecker urlChecker, 
        string imageUrl, 
        string checkedInputUrl,
        IFlickrResponse flickrResponse)
    {
        string htmlResponse;
        var checkedImageUrl = urlChecker.CheckImageUrl(imageUrl);

        var removeStartString = imageUrl.Remove(0, 30);
        var resultString = removeStartString.Remove(11, removeStartString.Length - 11);

        StringBuilder sb = new StringBuilder();
        var appendedUrl = sb.Append(checkedInputUrl + checkedImageUrl + "/sizes/");

        htmlResponse = flickrResponse.GetHtmlResponse(appendedUrl.ToString());

        var sizePathFromHtmlResponse = flickrResponse.GetSizePathFromHtmlResponse(htmlResponse);


        var imageHelper = new ImageHelper(sizePathFromHtmlResponse);
        var lastImagePair = imageHelper.GetImagePair();

        var lastImageSize = lastImagePair.Key;
        var lastImageUrl = "http://flickr.com" + lastImagePair.Value;

        var lastImageResponse = flickrResponse.GetHtmlResponse(lastImageUrl);
        var imgPathFromHtmlResponse = flickrResponse.GetImgPathFromHtmlResponse(lastImageResponse);


        var filteredUrl = flickrResponse.GetFilteredUrl(imgPathFromHtmlResponse);
        var fileName = filteredUrl.Split('/').Last();

        // Console.WriteLine("Quality: {0} Url: {1}", lastImageSize, lastImageUrl);

        IDownloader dl = new Downloader(filteredUrl, fileName);
        dl.Download();
        return fileName;
    }
}