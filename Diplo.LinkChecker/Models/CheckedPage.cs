using System;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;

namespace Diplo.LinkChecker.Models
{
    /// <summary>
    /// Represents an Umbraco page that can be checked
    /// </summary>
    public class CheckedPage : CheckedItem
    {
        /// <summary>
        /// Instantiate a new page to check from Umbraco content
        /// </summary>
        /// <param name="content">The content to instantiate the page from</param>
        public CheckedPage(IPublishedContent content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("The Umbraco content page to check was null");
            }

            this.CheckedLinks = Enumerable.Empty<Link>();

            this.Id = content.Id;
            this.Name = content.Name;
            this.UpdateDate = content.UpdateDate;
            this.UpdateUser = content.WriterName;
            this.Url = new Uri(content.Url, UriKind.RelativeOrAbsolute).ToString();
        }

        /// <summary>
        /// Get or set the Id of the page being checked
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Get or set the name of the page being checked
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get or set the date the page being checked was last updated
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// Get or set the user name of the person who last updated the page being checked
        /// </summary>
        public string UpdateUser { get; set; }

        /// <summary>
        /// Get or set the URL of the page being checked
        /// </summary>
        public new string Url { get; set; }

        /// <summary>
        /// Get or set the list of links within the page to be checked
        /// </summary>
        public IEnumerable<Link> CheckedLinks { get; set; }

        /// <summary>
        /// Get a count of how many links in this page have been checked
        /// </summary>
        public int LinksCount => this.CheckedLinks.Count();

        /// <summary>
        /// Get a count of how many links returned an error code
        /// </summary>
        public int ErrorCount => this.CheckedLinks.Where(x => !x.IsSuccessCode).Count();

        /// <summary>
        /// Get a count of how many links returned an error code matching what should be displayed
        /// </summary>
        public int DisplayCount => this.CheckedLinks.Where(x => x.IsDisplayCode).Count();

        /// <summary>
        /// Get a count of how many links returned a success code
        /// </summary>
        public int SuccessCount => this.CheckedLinks.Where(x => x.IsSuccessCode).Count();

        /// <summary>
        /// Get whether any of the links raised an error
        /// </summary>
        public bool HasErrors => this.CheckedLinks.Any(x => !x.IsSuccessCode);
    }
}