using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplo.LinkChecker.Helpers
{
    /// <summary>
    /// Simple helper for mapping a type of content to a more human-friendly version
    /// </summary>
    internal static class ContentMapHelper
    {
        internal static Dictionary<string, string> TagNameMap = new Dictionary<string, string>()
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

        internal static Dictionary<string, string> ContentTypeMap = new Dictionary<string, string>()
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
