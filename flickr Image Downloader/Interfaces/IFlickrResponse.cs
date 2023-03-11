namespace flickr_Image_Downloader.Interfaces;

public interface IFlickrResponse
{
    string GetHtmlResponse(string url);

    IEnumerable<string> GetImgPathFromHtmlResponse(string html);

    IEnumerable<string> GetSizePathFromHtmlResponse(string html);

    string GetFilteredUrl(IEnumerable<string> imgPathFromHtmlResponse);

    IEnumerable<string> GetImageUrls();

}

