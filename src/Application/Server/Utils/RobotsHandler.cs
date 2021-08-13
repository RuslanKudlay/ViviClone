using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Application.Utils
{
    public class RobotsHandler 
    {
        public bool IsReusable { get { return false; } }
        public void ProcessRequest(HttpContext context)
        {
            //string domain = context.Request.Url.Host;
            //// set the response code, content type and appropriate robots file here
            //// also think about handling caching, sending error codes etc.
            //context.Response.StatusCode = 200;
            //context.Response.ContentType = "text/plain";

            //StringBuilder stringBuilder = new StringBuilder();

            //stringBuilder.AppendLine("user-agent: *");
            //stringBuilder.AppendLine("disallow: ");
            //stringBuilder.AppendLine("");
            //stringBuilder.Append("Sitemap: " + PathUtils.CombinePaths(Microsoft.AspNetCore.HttpContext.Request.Host.ToString(), "/sitemap"));

            //context.Response.WriteAsync(stringBuilder.ToString());
        }
    }
}