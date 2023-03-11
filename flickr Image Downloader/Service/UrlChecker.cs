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


}