using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Diplo.LinkChecker.Models
{
    public abstract class CheckedItem
    {
        public Uri Url { get; set; }

        public string Status { get; set; }

        public string StatusCode { get; set; }

        public bool IsSuccessCode { get; set; }

        public string Error { get; set; }
    }
}
