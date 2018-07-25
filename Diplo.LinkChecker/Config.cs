using System;
using System.Net;
using System.Net.Http;

namespace Diplo.LinkChecker
{
    internal static class Config
    {
        /// <summary>
        /// Gets an instance of the client handler that has configuration values for the HTTP services
        /// </summary>
        /// <returns>An HTTP client handler</returns>
        public static HttpClientHandler GetClientHandler()
        {
            return new HttpClientHandler()
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
        }
    }
}
