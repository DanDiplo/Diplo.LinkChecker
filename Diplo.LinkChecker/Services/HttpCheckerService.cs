using Diplo.LinkChecker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;

namespace Diplo.LinkChecker.Services
{

    public class HttpCheckerService
    {
        public string UserAgent { get; set; }

        public HttpCheckerService()
        {
            this.UserAgent = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
        }


        public async Task<string> GetHtmlFromUrl(Uri url)
        {
            if (url == null)
            {
                throw new ArgumentNullException("url", "The URL to fetch HTML from cannot be null");
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("user-agent", this.UserAgent);

                using (var message = new HttpRequestMessage(HttpMethod.Get, url))
                {
                    using (var response = await client.SendAsync(message))
                    {
                        response.EnsureSuccessStatusCode();

                        if (response.IsSuccessStatusCode)
                        {
                            return await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }

            return String.Empty;
        }

        public async Task<IEnumerable<Link>> CheckLinks(IEnumerable<Link> itemsToCheck)
        {
            var handler = new HttpClientHandler()
            {
                AllowAutoRedirect = true,
            };

            using (HttpClient client = new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Add("user-agent", this.UserAgent);

                IEnumerable<Task<Link>> checkUrlsQuery =
                    from item in itemsToCheck select CheckUrl(client, item);

                Task<Link>[] itemTasks = checkUrlsQuery.ToArray();

                var results = await Task.WhenAll(itemTasks);

                return results;
            }
        }

        private async Task<Link> CheckUrl(HttpClient client, Link link)
        {
            var existing = MemoryCache.Default.Get(link.Url.AbsoluteUri) as Link;

            if (existing != null)
            {
                existing.CheckedPreviously = true;
                return existing;
            }

            try
            {
                using (var message = new HttpRequestMessage(HttpMethod.Head, link.Url))        
                {
                    using (var response = await client.SendAsync(message))
                    {
                        link.Status = response.ReasonPhrase;
                        link.StatusCode = ((int)response.StatusCode).ToString();

                        link.IsSuccessCode = response.IsSuccessStatusCode;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                if (ex.InnerException != null && ex.InnerException is WebException)
                {
                    link.Status = ex.Message;

                    var webEx = ex.InnerException as WebException;

                    if (webEx != null)
                    {
                        link.Status += " " + webEx.Message;
                    }
                }
                else
                {
                    link.Error = ex.Message;
                }
            }
            catch (Exception ex)
            {
                link.Error = link.Status = ex.Message;
            }

            MemoryCache.Default.Add(link.Url.AbsoluteUri, link, DateTime.Now.AddMinutes(1));


            return link;
        }
    }
}
