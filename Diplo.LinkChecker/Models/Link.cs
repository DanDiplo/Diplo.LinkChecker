using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Umbraco.Core;

namespace Diplo.LinkChecker.Models
{
    public class Link : CheckedItem
    {
        public string Text { get; set; }

        public string Attribute { get; set; }

        public string TagName { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public bool Internal { get; set; }

        public bool CheckedPreviously { get; set; }

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
