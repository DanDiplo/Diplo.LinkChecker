using System;
using Diplo.LinkChecker.Helpers;
using Umbraco.Core;

namespace Diplo.LinkChecker.Models
{
    /// <summary>
    /// Represents an individual link that can be checked
    /// </summary>
    public class Link : CheckedItem
    {
        /// <summary>
        /// Get or set the optional text that is part of the link
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Get or set the attribute name that the link is from eg. "href" or "src"
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// Get or set the name of the tag that forms the link eg. "a" or "img"
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Get or set the line number in the HTML that the link was found on
        /// </summary>
        public int Line { get; set; }

        /// <summary>
        /// Get or set the column number in the HTML that the link was found on
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        /// Get or set whether this was an internal link being checked
        /// </summary>
        public bool IsInternal { get; set; }

        /// <summary>
        /// Get or set whether this link has been checked recently
        /// </summary>
        public bool CheckedPreviously { get; set; }

        /// <summary>
        /// Get the type of link this is eg. "Hyperlink" or "Resource"
        /// </summary>
        public string LinkType
        {
            get
            {
                if (String.IsNullOrEmpty(this.TagName))
                {
                    return String.Empty;
                }

                if (!ContentMapHelper.TagNameMap.TryGetValue(this.TagName, out string type))
                {
                    type = this.TagName.ToFirstUpper();
                }

                return type;
            }
        }

        /// <summary>
        /// Gets the friendly content-type name
        /// </summary>
        public string TypeName
        {
            get
            {
                if (String.IsNullOrEmpty(this.ContentType))
                {
                    return String.Empty;
                }

                if (!ContentMapHelper.ContentTypeMap.TryGetValue(this.ContentType, out string type))
                {
                    type = this.ContentType.Substring(0, this.ContentType.IndexOf('/')).ToFirstUpper();
                }

                return type;
            }
        }

        /// <summary>
        /// Returns a string representation of this link
        /// </summary>
        public override string ToString()
        {
            return String.Format("Url: {0}, Text: {1}, Tag: {2}, Attribute: {3}, Internal?: {4}, Position: ({5},{6})",
                this.Url, this.Text, this.TagName, this.Attribute, this.IsInternal, this.Line, this.Column);
        }
    }
}