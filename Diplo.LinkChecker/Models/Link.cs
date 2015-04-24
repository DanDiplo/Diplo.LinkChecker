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

                string type;
                if (!TagNameMap.TryGetValue(this.TagName, out type))
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

                string type;
                if (!ContentTypeMap.TryGetValue(this.ContentType, out type))
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
            return String.Format("Url: {0}, Text: {1}, Tag: {2}, Attribute: {3}, Internal?: {4}, Position: ({5},{6})", this.Url, this.Text, this.TagName, this.Attribute, this.IsInternal, this.Line, this.Column);
        }

        private static Dictionary<string, string> TagNameMap = new Dictionary<string, string>()
        {
            { "link", "Resource" },
            { "script", "Script" },
            { "img", "Image" },
            { "a", "Hyperlink" },
            { "audio", "Audio" },
            { "embed", "Embedded Media" },
            { "object", "Embedded Object" },
            { "video", "Video" },
            { "form", "Form" },
            { "iframe", "iFrame" },
            { "area", "Area Shape" },
            { "base", "Base URL"}
        };

        private static Dictionary<string, string> ContentTypeMap = new Dictionary<string, string>()
        {
            { "application/atom+xml", "Atom Feed" },
            { "application/javascript", "JavaScript" },
            { "text/javascript", "JavaScript" },
            { "application/pdf", "PDF Doc" },
            { "application/rss+xml", "RSS Feed" },
            { "application/font-woff", "Web Font" },
            { "application/xml", "XML Document" },
            { "image/gif", "GIF Image" },
            { "image/jpg", "JPEG Image" },
            { "image/jpeg", "JPEG Image" },
            { "image/png", "PNG Image" },
            { "image/svg+xml", "Vector Image" },
            { "image/bmp", "Bitmap Image" },
            { "text/css", "Style Sheet"},
            { "text/html", "HTML Page"},
            { "text/plain", "Plain Text"},
            { "text/rtf", "Rich Text"},
            { "video/mpeg", "MPEG1 Video"},
            { "video/mp4", "MP4 Video"},
            { "video/x-flv", "Flash Video"},
            { "application/x-shockwave-flash", "Flash Movie"},
            { "application/vnd.ms-excel", "MS Excel"},
            { "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "MS Excel"},
            { "application/vnd.ms-powerpoint", "MS Powerpoint"},
            { "application/vnd.openxmlformats-officedocument.presentationml.presentation", "MS Powerpoint"},
            { "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "MS Word"},
            { "application/vnd.ms-project", "MS Project"},
            { "application/msword", "MS Word"},
            { "audio/mpeg", "MP3 Audio"},
            { "audio/mp4", "MP4 Audio"},
            { "application/xhtml+xml", "XHTML Page"},
            { "application/zip", "ZIP Archive"},
            { "image/x-icon", "Icon Image"},
            { "video/quicktime", "QuickTime Video"},
            { "application/octet-stream", "Binary File"},
            { "video/h264", "H264 Video"},
            { "text/calendar", "Calendar Event" },
            { "application/x-silverlight-app", "MS Silverlight" },
            { "video/x-ms-wmv", "Windows Media" }
        };

    }
}
