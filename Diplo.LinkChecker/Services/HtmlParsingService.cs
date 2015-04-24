using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Diplo.LinkChecker.Models;
using HtmlAgilityPack;

namespace Diplo.LinkChecker.Services
{
    /// <summary>
    /// A service for parsing HTML for specific elements (currently "links")
    /// </summary>
    public class HtmlParsingService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlParsingService"/> class from the base URL of the site
        /// </summary>
        /// <param name="baseUri">The base URI of the site being checked</param>
        public HtmlParsingService(Uri baseUri)
        {
            if (baseUri == null)
            {
                throw new ArgumentNullException("The base URL of the site being checked cannot be null");
            }

            this.BaseUri = baseUri;
        }

        private static Regex MatchProtocolRegex = new Regex(@"^\w{3,8}://*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly string[] InvalidProtocols = { "mailto:", "ftp:", "tel:", "ldap://", "ftp://", "mms://", "news:", "nntp://", "sftp://", "ssh://", "telnet://", "udp://", "javascript:" };

        /// <summary>
        /// If true then only considers the main document body, otherwise uses entire HTML document
        /// </summary>
        public bool OnlyCheckBody { get; set; }

        /// <summary>
        /// Get the base URL of the site being checked
        /// </summary>
        public Uri BaseUri { get; private set; }

        /// <summary>
        /// Gets the links from HTML document to check. These could be hyperlinks, images, JavaScript assets etc.
        /// </summary>
        /// <param name="html">The HTML to parse</param>
        /// <returns>Yields the links that are parsed</returns>
        public IEnumerable<Link> GetLinksFromHtmlDocument(string html)
        {
            if (String.IsNullOrEmpty(html))
            {
                yield break;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);

            if (doc == null || doc.DocumentNode == null)
            {
                yield break;
            }

            string xPath = this.OnlyCheckBody ? "//body//*[@src or @href]" : "//*[@src or @href]";

            var linkNodes = doc.DocumentNode.SelectNodes(xPath);

            foreach (HtmlNode node in linkNodes)
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

                if (source != null && !String.IsNullOrEmpty(source.Value) && source.Value.Trim() != "#")
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
                            link.IsInternal = !MatchProtocolRegex.IsMatch(source.Value);

                            yield return link;
                        }
                    }
                }
            }
        }
    }
}
