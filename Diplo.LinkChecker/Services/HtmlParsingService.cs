using Diplo.LinkChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace Diplo.LinkChecker.Services
{
    public class HtmlParsingService
    {
        private static Regex MatchProtocolRegex = new Regex(@"^\w{3,8}://*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static string[] InvalidProtocols = { "mailto:", "ftp:", "tel:" };

        public HtmlParsingService(Uri baseUri)
        {
            this.BaseUri = baseUri;
        }


        public Uri BaseUri { get; private set; }


        public IEnumerable<Link> GetLinksFromHtmlDocument(string html)
        {
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            foreach (HtmlNode node in doc.DocumentNode.SelectNodes("//*[@src or @href]"))
            {
                HtmlAttribute source;
                string attribute;

                if (node.Attributes.Contains("href"))
                {
                    source = node.Attributes["href"];
                    attribute = "href";
                }
                else if (node.Attributes.Contains("src"))
                {
                    source = node.Attributes["src"];
                    attribute = "src";
                }
                else
                {
                    continue;
                }

                if (!String.IsNullOrEmpty(source.Value) && source.Value.Trim() != "#")
                {
                    Uri uri;

                    if (Uri.TryCreate(this.BaseUri, source.Value, out uri))
                    {
                        if (uri.Scheme == "http" || uri.Scheme == "https")
                        {
                            Link link = new Link();
                            link.Url = uri;
                            link.Text = node.InnerText;
                            link.Line = source.Line;
                            link.Column = source.LinePosition;
                            link.Attribute = attribute;
                            link.TagName = node.Name;
                            link.Internal = !MatchProtocolRegex.IsMatch(source.Value);

                            yield return link;
                        }
                    }
                }
            }
        }
    }
}
