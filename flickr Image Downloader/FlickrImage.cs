namespace flickr_Image_Downloader;

public class FlickrImage
{
    public  string ImageUrl { get; set; }

    public string HtmlResponse { get; set; }

    public string FileName { get; set; }

    public string FilteredUrl { get; set; }

    public KeyValuePair<string, string> ImagePair { get; set; }

    public string ImageSize { get; set; }

    public string AppendedUrl { get; set; }

    public IList<string> ImageSizePathes { get; set; }


    public FlickrImage(string imageUrl)
    {
        ImageUrl = imageUrl;
    }

    public string GetFileName()
    {
        return FilteredUrl.Split('/').Last();
    }
}