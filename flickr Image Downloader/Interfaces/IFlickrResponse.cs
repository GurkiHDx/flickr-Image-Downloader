namespace flickr_Image_Downloader.Interfaces;

public interface IFlickrResponse
{
    string GetHtmlResponse(string url);

    string GetHtml(string url);

    IEnumerable<string> GetImgPathFromHtmlResponse(string html);

    IEnumerable<string> GetSizePathFromHtmlResponse(string html);

    string GetFilteredUrl(IEnumerable<string> imgPathFromHtmlResponse);

}

