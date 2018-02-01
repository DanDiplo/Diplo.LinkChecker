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
        /// <returns>A list of checked links</returns>
        /// <remarks>
        /// /Umbraco/Backoffice/Api/LinkChecker/CheckPage/1073
        /// </remarks>
        [HttpGet]
        public async Task<CheckedPage> CheckPage(int id, bool checkEntireDocument = false, int timeout = 30, bool omitPortDuringChecks = false)
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
                page.CheckedLinks = await checker.CheckLinks(links);
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
