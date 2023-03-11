using System.Text;

namespace flickr_Image_Downloader.Service;

public class UrlChecker
{
    public string CheckInputUrl(string inputUrl)
    {
        string checkedUrl = inputUrl;

        if (inputUrl.Contains("page"))
        {
            var urlArray = inputUrl.Split('/');

            StringBuilder sb = new StringBuilder();
            foreach (var s in urlArray)
            {
                if (!s.Contains("page"))
                {
                    sb.Append(s + '/');
                }
            }

            checkedUrl = sb.ToString();
        }

        if (!checkedUrl.EndsWith('/'))
        {
            checkedUrl += "/";
        }

        return checkedUrl;
    }

    public string CheckImageUrl(string imageUrl)
    {
        var urlArray = imageUrl.Split('/');
        var last = urlArray.Last();
        var x = last.Split('_');
        var first = x.First();
        return first;
    }


    public string BuildImageUrl(string checkedInputUrl, string imageUrl)
    {
        var checkedImageUrl = CheckImageUrl(imageUrl);

        var removeStartString = imageUrl.Remove(0, 30);
        var resultString = removeStartString.Remove(11, removeStartString.Length - 11);

        StringBuilder sb = new StringBuilder();
        var appendedUrl = sb.Append(checkedInputUrl + checkedImageUrl + "/sizes/").ToString();

        return appendedUrl;
    }


}