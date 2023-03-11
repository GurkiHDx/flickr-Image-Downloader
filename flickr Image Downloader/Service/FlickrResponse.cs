using System.Configuration;
using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System.Net;
using flickr_Image_Downloader.Interfaces;

namespace flickr_Image_Downloader.Service;

public class FlickrResponse : IFlickrResponse
{
    private readonly string _inputUrl;

    public FlickrResponse(string inputUrl)
    {
        _inputUrl = inputUrl;
    }

    public string GetHtmlResponse(string url)
    {
        string htmlResponse;
        using (WebClient wc = new WebClient())
        {
            htmlResponse = wc.DownloadString(url);
        }

        return htmlResponse;
    }

    private string GetChromePageSource(string url)
    {
        var options = new ChromeOptions
        {
            BinaryLocation = ConfigurationManager.AppSettings["chromeLocation"]
        };

        options.AddArguments("headless");

        var chrome = new ChromeDriver(options);
        chrome.Manage().Window.Maximize();


        chrome.Navigate().GoToUrl(url);

        var timeout = 10000; /* Maximum wait time of 10 seconds */
        var wait = new WebDriverWait(chrome, TimeSpan.FromMilliseconds(timeout));
        wait.Until(d => ((IJavaScriptExecutor)d).ExecuteScript("return document.readyState").Equals("complete"));

        Thread.Sleep(2000);

        /* Once the page has loaded, scroll to the end of the page to load all the videos */
        /* Scroll to the end of the page to load all the videos in the channel */
        /* Reference - https://stackoverflow.com/a/51702698/126105 */
        /* Get scroll height */
        Int64 last_height = (Int64)(((IJavaScriptExecutor)chrome).ExecuteScript("return document.documentElement.scrollHeight"));
        while (true)
        {
            ((IJavaScriptExecutor)chrome).ExecuteScript("window.scrollTo(0, document.documentElement.scrollHeight);");
            /* Wait to load page */
            Thread.Sleep(2000);
            /* Calculate new scroll height and compare with last scroll height */
            Int64 new_height = (Int64)((IJavaScriptExecutor)chrome).ExecuteScript("return document.documentElement.scrollHeight");
            if (new_height == last_height)
                /* If heights are the same it will exit the function */
                break;
            last_height = new_height;
        }

        return chrome.PageSource;
    }

    public IEnumerable<string> GetImgPathFromHtmlResponse(string html)
    {
        HtmlDocument htmlSnippet = new HtmlDocument();
        htmlSnippet.LoadHtml(html);

        List<string> imgTags = new List<string>();

        foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//img"))
        {
            HtmlAttribute att = link.Attributes["src"];
            imgTags.Add(att.Value);
        }

        return imgTags;
    }

    public IEnumerable<string> GetSizePathFromHtmlResponse(string html)
    {
        HtmlDocument htmlSnippet = new HtmlDocument();
        htmlSnippet.LoadHtml(html);

        List<string> hrefTags = new List<string>();

        foreach (HtmlNode link in htmlSnippet.DocumentNode.SelectNodes("//a[@href]"))
        {
            HtmlAttribute att = link.Attributes["href"];

            if (att.Value.Contains("sizes"))
            {
                hrefTags.Add(att.Value);
            }
        }

        return hrefTags;
    }

    public string GetFilteredUrl(IEnumerable<string> imgPathFromHtmlResponse)
    {
        return imgPathFromHtmlResponse.FirstOrDefault(x => x.Contains("live.staticflickr"));
    }

    public IEnumerable<string> GetImageUrls()
    {
        var htmlResponse = GetChromePageSource(_inputUrl);
        var imageUrls = GetImgPathFromHtmlResponse(htmlResponse);
        return imageUrls;
    }
}