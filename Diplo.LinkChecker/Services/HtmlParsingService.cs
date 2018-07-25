using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
        /// <param name="allowNonStandardPortInBaseUri">If true, when checking links found in a document, a non-standard port be omitted from the request.</param>
        public HtmlParsingService(Uri baseUri, bool allowNonStandardPortInBaseUri = true)
        {
            if (baseUri == null) throw new ArgumentNullException("The base URL of the site being checked cannot be null");

            this.BaseUri = allowNonStandardPortInBaseUri ? baseUri : new UriBuilder(baseUri) { Port = -1 }.Uri;
        }

        private static Regex MatchProtocolRegex = new Regex(@"^\w{3,8}://*", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        // https://www.iana.org/assignments/uri-schemes/uri-schemes.xhtml
        private static readonly string[] InvalidProtocols = { "mailto:", "ftp:", "tel:", "ldap://", "urn:", "magnet:", "ftp://", "geo:", "mms://", "news:", "nntp://", "sftp://", "ssh://", "telnet://", "udp://", "javascript:", "webcal:", "chrome://" };

        /// <summary>
        /// If true then parses the entire HTML document, otherwise just checks the BODY section
        /// </summary>
        public bool CheckEntireDocument { get; set; }

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

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            if (doc == null || doc.DocumentNode == null)
            {
                yield break;
            }

            string xPath = this.CheckEntireDocument ? "//*[@src or @href]" : "//body//*[@src or @href]";

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
                    if (Uri.TryCreate(this.BaseUri, source.Value, out Uri uri))
                    {
                        if (uri.Scheme == "http" || uri.Scheme == "https")
                        {
                            var link = new Link
                            {
                                Url = uri,
                                Text = node.InnerText,
                                Line = source.Line,
                                Column = source.LinePosition,
                                Attribute = attribute,
                                TagName = node.Name,
                                IsInternal = !MatchProtocolRegex.IsMatch(source.Value)
                            };

                            yield return link;
                        }
                    }
                }
            }
        }
    }
}