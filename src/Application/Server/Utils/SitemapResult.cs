using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;

namespace Application.Utils
{
    public class SitemapResult : ActionResult
    {
        private readonly IEnumerable<ISitemapItem> items;
        private readonly ISitemapGenerator generator;

        public SitemapResult(IEnumerable<ISitemapItem> items) : this(items, new SitemapGenerator())
        {
        }

        public SitemapResult(IEnumerable<ISitemapItem> items, ISitemapGenerator generator)
        {
            this.items = items;
            this.generator = generator;
        }

        public override void ExecuteResult(ActionContext context)
        {
            var response = context.HttpContext.Response;

            response.ContentType = "text/xml";
            //response = Encoding.UTF8;

            using (var writer = new XmlTextWriter(new System.IO.StreamWriter(response.Body)))
            {
                writer.Formatting = Formatting.Indented;
                var sitemap = generator.GenerateSiteMap(items);

                sitemap.WriteTo(writer);
            }
        }
    }
}