using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Diplo.LinkChecker.Models;
using Diplo.LinkChecker.Services;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.WebApi;

namespace Diplo.LinkChecker.Controllers
{
    using Lucene.Net.Search;

    /// <summary>
    /// Controller for checking links and related tasks
    /// </summary>
    public class LinkCheckerController : UmbracoAuthorizedApiController
    {
        /// <summary>
        /// Checks all the links in the page with the given node Id
        /// </summary>
        /// <param name="id">The Id of the node to check</param>
        /// <param name="checkEntireDocument">If false only checks the HTML page BODY; otherwise checks entire page (default is false)</param>
        /// <param name="timeout">The timeout period (in seconds) before the checker abandons trying to connect to a server</param>
        /// <param name="omitPortDuringChecks">Omits the port number when checking a page</param>
        /// <param name="checkInternalLinksOnly">Skip checking external links</param>
        /// <param name="onlyShowErrors">Display only errors, not 200s</param>
        /// <param name="showErrors">Error code types to display (default = 300,400,500)</param>
        /// <returns>A list of checked links</returns>
        /// <remarks>
        /// /Umbraco/Backoffice/Api/LinkChecker/CheckPage/1073
        /// </remarks>
        [HttpGet]
        public async Task<CheckedPage> CheckPage(int id, bool checkEntireDocument = false, int timeout = 30, bool omitPortDuringChecks = false, bool checkInternalLinksOnly = false, bool onlyShowErrors = false, string showErrors = "300,400,500")
        {
            var node = Umbraco.TypedContent(id);

            if (node == null)
            {
                throw new ArgumentOutOfRangeException("No node could be found with an id of " + id);
            }

            if (timeout < 1)
            {
                timeout = 1;
            }

            CheckedPage page = new CheckedPage(node);

            HttpCheckerService checker = new HttpCheckerService();
            checker.Timeout = TimeSpan.FromSeconds(timeout);

            string html = await checker.GetHtmlFromUrl(new Uri(node.UrlAbsolute()));

            HtmlParsingService parser = new HtmlParsingService(new Uri(Request.RequestUri.GetLeftPart(UriPartial.Authority)), !omitPortDuringChecks);
            parser.CheckEntireDocument = checkEntireDocument;

            var links = parser.GetLinksFromHtmlDocument(html);

            if (links != null && links.Any())
            {
                if (checkInternalLinksOnly)
                {
                    var internalLinks = links.Where(n => n.IsInternal);
                    page.CheckedLinks = await checker.CheckLinks(internalLinks);
                }
                else
                {
                    page.CheckedLinks = await checker.CheckLinks(links);
                }
                
            }

            //Set Display based on error type & options
            var displayAllErrors = showErrors ==  "300,400,500";
            var displayErrorCodes = showErrors.Split(',').ToList();
            var displayPrefixes = new List<string>();
            if (!onlyShowErrors)
            {
                displayErrorCodes.Add("200");
            }

            foreach (var code in displayErrorCodes)
            {
                if (code.Length > 0)
                {
                    var prefix = code.Substring(0, 1);
                    displayPrefixes.Add(prefix);
                }
            }

            if (page.CheckedLinks!=null && page.CheckedLinks.Any())
            {
                var updatedLinks = new List<Link>();

                //var linksWithCodes = page.CheckedLinks.Where(n => !string.IsNullOrEmpty(n.StatusCode)).ToList();
                //var linksWithOutCodes = page.CheckedLinks.Where(n => string.IsNullOrEmpty(n.StatusCode)).ToList();

                foreach (var checkedLink in page.CheckedLinks.ToList())
                {
                    //Link with status code
                    if (!string.IsNullOrEmpty(checkedLink.StatusCode))
                    {
                        var statusCodePrefix = checkedLink.StatusCode.Substring(0, 1);
                        if (displayPrefixes.Contains(statusCodePrefix))
                        {
                            checkedLink.IsDisplayCode = true;
                        }
                        else
                        {
                            checkedLink.IsDisplayCode = false;
                        }
                    }
                    else //Link without status code
                    {
                        if (displayAllErrors)
                        {
                            //Also show errors without a code
                            checkedLink.IsDisplayCode = true;
                        }
                        else
                        {
                            checkedLink.IsDisplayCode = false;
                        }
                    }

                    updatedLinks.Add(checkedLink);
                }


                //foreach (var code in displayErrorCodes)
                //{
                //    if (code.Length > 0)
                //    {
                //        var prefix = code.Substring(0, 1);
                //        var matches = linksWithCodes.Where(n => n.StatusCode.StartsWith(prefix)).ToList();
                //        if (matches.Any())
                //        {
                //            foreach (var match in matches)
                //            {
                //                match.IsDisplayCode = true;
                //                updatedLinks.Add(match);
                //            }
                //        }
                //    }
                //}

                //if (displayAllErrors)
                //{
                //    //Also show errors without a code
                //    foreach (var match in linksWithOutCodes)
                //    {
                //        match.IsDisplayCode = true;
                //        updatedLinks.Add(match);
                //    }
                //}

                page.CheckedLinks = updatedLinks;
            }

            return page;
        }

        /// <summary>
        /// Gets a list of node Ids that are descendants from the given start node
        /// </summary>
        /// <param name="id">The start node Id</param>
        /// <returns>A list of node Ids</returns>
        /// <remarks>
        /// /Umbraco/Backoffice/Api/LinkChecker/GetIdsToCheck/1073
        /// </remarks>
        [HttpGet]
        public IEnumerable<int> GetIdsToCheck(int id)
        {
            var startNode = Umbraco.TypedContent(id);

            if (startNode == null)
            {
                throw new ArgumentOutOfRangeException("No node could be found with an id of " + id);
            }

            return startNode.DescendantsOrSelf().Where(n => n.TemplateId > 0).Select(n => n.Id);
        }
    }
}
