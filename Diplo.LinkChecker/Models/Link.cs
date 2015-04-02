using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
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
        public bool Internal { get; set; }

        /// <summary>
        /// Get or set whether this link has been checked recently
        /// </summary>
        public bool CheckedPreviously { get; set; }

        /// <summary>
        /// Get the type of link this is eg. "Hyperlink" or "Resource"
        /// </summary>
        public string Type 
        {
            get
            {
                if (String.IsNullOrEmpty(this.TagName))
                {
                    return String.Empty;
                }

                string type;
                if (!TagNameMap.TryGetValue(this.TagName, out type))
                {
                    type = this.TagName.ToFirstUpper();
                }

                return type;
            }
        }

        /// <summary>
        /// Returns a string representation of this link
        /// </summary>
        /// <returns>
        public override string ToString()
        {
            return String.Format("Url: {0}, Text: {1}, Tag: {2}, Attribute: {3}, Internal?: {4}, Position: ({5},{6})", this.Url, this.Text, this.TagName, this.Attribute, this.Internal, this.Line, this.Column);
        }

        private static Dictionary<string, string> TagNameMap = new Dictionary<string, string>()
        {
            { "link", "Resource" },
            { "script", "JavaScript" },
            { "img", "Image" },
            { "a", "Hyperlink" },
            { "audio", "Audio" },
            { "embed", "Embedded Media" },
            { "object", "Embedded Object" },
            { "video", "Video" },
            { "form", "Form" },
            { "iframe", "iFrame" },
            { "area", "Area" },
            { "base", "Base URL"}
        };
    }
}
