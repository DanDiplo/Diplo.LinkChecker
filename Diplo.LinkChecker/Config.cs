using System;
using System.Net;
using System.Net.Http;

namespace Diplo.LinkChecker
{
    internal static class Config
    {
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
