using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Diplo.LinkChecker.Models
{
    /// <summary>
    /// Base class for anything that can be linked checked
    /// </summary>
    public abstract class CheckedItem
    {
        /// <summary>
        /// Get or set the full URL to check
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>
        /// Get or set the status of the link eg. Not Found
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get or set the status code eg. 404
        /// </summary>
        /// <remarks>
        /// This is a string as makes JSON serialisation a bit simpler
        /// </remarks>
        public string StatusCode { get; set; }

        /// <summary>
        /// Get or set whether the status code indicates a successful check
        /// </summary>
        public bool IsSuccessCode { get; set; }

        /// <summary>
        /// Get or set any additional error information eg. exception message
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Get or set the content type of the URL
        /// </summary>
        public string ContentType { get; set; }
    }
}
