using System.Net;
namespace MasterdevZad1.ReadHTMLClassLibrary


{
    public static class ReadHTML
    {
        public static string Send(string url, string node)
        {
            try
            {
                if (!string.IsNullOrEmpty(url))
                {
                    WebClient webClient = new WebClient();
                    webClient.Encoding = System.Text.Encoding.UTF8;
                    string html = webClient.DownloadString(url);
                    if(!string.IsNullOrEmpty(html))
                    {
                        HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
                        htmlDocument.LoadHtml(html);
                        if(htmlDocument != null)
                        {

                            //HtmlAgilityPack.HtmlNode htmlNode= htmlDocument.DocumentNode.SelectSingleNode(node);
                            //if(htmlNode != null)
                            //{
                            //    htmlNode = htmlNode.LastChild;

                            //    string innerText = htmlNode.InnerText;

                            //    if(!string.IsNullOrEmpty(innerText))
                            //    {
                            //        return innerText;
                            //    }
                            //}
                            HtmlAgilityPack.HtmlNodeCollection htmlNodes = htmlDocument.DocumentNode.SelectNodes(node);
                            string innerText = htmlNodes[47].InnerText;
                            

                            
                            if(!string.IsNullOrEmpty(innerText))
                            {
                                return innerText;
                            }
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}
