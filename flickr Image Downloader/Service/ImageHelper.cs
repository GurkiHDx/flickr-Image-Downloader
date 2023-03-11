using OpenQA.Selenium.DevTools.V108.Page;

namespace flickr_Image_Downloader.Service;

public class ImageHelper
{
    private readonly IEnumerable<string>? _sizePathFromHtmlResponse;

    public ImageHelper(IEnumerable<string>? sizePathFromHtmlResponse)
    {
        _sizePathFromHtmlResponse = sizePathFromHtmlResponse;
    }

    public KeyValuePair<string, string> GetImagePair()
    {
        Dictionary<string, string> imageSizesWithUrl = new Dictionary<string, string>();

        foreach (var sizePathUrl in _sizePathFromHtmlResponse)
        {
            foreach (var imageSize in GetImageSizesDictionary())
            {
                if (sizePathUrl.Contains("/" + imageSize.Key + "/"))
                {
                    imageSizesWithUrl.Add(imageSize.Key, sizePathUrl);
                    break;
                }
            }
        }

        KeyValuePair<string, string> lastImagePair;
        if (imageSizesWithUrl.ContainsKey("o"))
        {
            lastImagePair = imageSizesWithUrl.First();
        }
        else
        {
            lastImagePair = imageSizesWithUrl.Last();
        }

        return lastImagePair;
    }

    private Dictionary<string, string> GetImageSizesDictionary()
    {
        Dictionary<string, string> imageSizes = new Dictionary<string, string>()
        {
            // Quadrat, 75(75 × 75)
            // Quadrat, 150(150 × 150)
            // Thumbnail(100 × 67)
            // Klein, 240(240 × 160)
            // Klein, 320(320 × 213)
            // Klein 400(400 × 267)
            // Mittel 500(500 × 333)
            // Mittel, 640(640 × 427)
            // Mittel, 800(800 × 534)
            // Groß, 1024(1024 × 683)
            // Groß, 1600(1600 × 1067)
            // Groß, 2048(2048 × 1366)
            // Extragroß 3 K(3072 × 2049)
            // Extragroß 4 K(4096 × 2732)
            // Extragroß 5 K(5120 × 3415)
            // Extragroß 6 K(6144 × 4098)
            // Original(8192 × 5464)

            { "sq", "Quadrat 75 (75 × 75)" },
            { "q", "Quadrat 150 (150 × 150)" },
            { "t", "Thumbnail (100 × 67)" },
            { "s", "Klein 240 (240 × 160)" },
            { "n", "Klein 320 (320 × 213)" },
            { "w", "Klein 400 (400 × 267)" },
            { "m", "Mittel 500 (500 × 333)" },
            { "z", "Mittel, 640 (640 × 427)" },
            { "c", "Mittel, 800 (800 × 534)" },
            { "l", "Groß, 1024 (1024 × 683)" },
            { "h", "Groß, 1600 (1600 × 1067)" },
            { "k", "Groß, 2048 (2048 × 1366)" },
            { "3k", "Extragroß 3 K(3072 × 2049)" },
            { "4k", "Extragroß 4 K (4096 × 2732)" },
            { "5k", "Extragroß 5 K (5120 × 3415)" },
            { "6k", "Extragroß 6 K (6144 × 4098)" },
            { "o", "Original (8192 × 5464)" }
        };
        return imageSizes;
    }
}