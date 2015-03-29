using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Umbraco.Core.Models;

namespace Diplo.LinkChecker.Models
{
    public class CheckedPage : CheckedItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime UpdateDate { get; set; }

        public string UpdateUser { get; set; }

        public string Url { get; set; }

        public IEnumerable<Link> CheckedLinks { get; set; }

        public int LinksCount 
        {
            get
            {
                return this.CheckedLinks.Count();
            }
        }

        public int ErrorCount 
        {
            get
            {
                return this.CheckedLinks.Where(x => !x.IsSuccessCode).Count();
            }
        }

        public int SuccessCount 
        {
            get
            {
                return this.CheckedLinks.Where(x => x.IsSuccessCode).Count();
            }
        }

        public bool HasErrors 
        {
            get
            {
                return this.CheckedLinks.Any(x => !x.IsSuccessCode);
            }
        }

        public CheckedPage(IPublishedContent content)
        {
            this.CheckedLinks = Enumerable.Empty<Link>();

            this.Id = content.Id;
            this.Name = content.Name;
            this.UpdateDate = content.UpdateDate;
            this.UpdateUser = content.WriterName;
            this.Url = content.Url;
        }
    }
}
