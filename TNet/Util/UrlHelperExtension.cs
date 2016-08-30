using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.Web.Routing;

namespace TNet.Util
{
    public static class UrlHelperExtension
    {
        

        public static string BootstrapPagerHTML(this UrlHelper url, int pageCount,int pageIndex, string actionName, string controllerName, RouteValueDictionary  routeValues)
        {
            StringBuilder sb = new StringBuilder();
            if (pageCount == 0)
            {
                throw new Exception("总页数不能为0.");
            }
            if (pageCount==1) {
                return "";
            }
            
           

            if (routeValues==null&& routeValues.Count>0) {
                string key= routeValues.Keys.Where(en => en == "pageIndex").FirstOrDefault();
                if (string.IsNullOrEmpty(key)) {
                    routeValues.Add("pageIndex",pageIndex);
                }
            }

            sb.AppendLine("<nav>");
            sb.AppendLine("<ul class=\"pagination\">");
            sb.AppendLine("<li [pre]>");
            routeValues["pageIndex"] = pageIndex - 1;
            sb.AppendLine(string.Format( "<a href = \"{0}\" aria-label=\"Previous\">", url.Action(actionName, controllerName, routeValues)));
            sb.AppendLine("<span aria-hidden=\"true\">&laquo;</span>");
            sb.AppendLine("</a>");
            sb.AppendLine("</li>");
            for (int i = 0; i < pageCount; i++)
            {
                routeValues["pageIndex"] = i;
                sb.AppendLine(string.Format( "<li {2}><a href = \"{0}\" > {1} </a></li>",url.Action(actionName,controllerName,routeValues),i+1, (pageIndex==i)?" class=\"active\"":""));
            }
           
            sb.AppendLine("<li [next]>");
            routeValues["pageIndex"] = pageIndex + 1;
            sb.AppendLine(string.Format( "<a href=\"{0}\" aria-label=\"Next\">", url.Action(actionName, controllerName, routeValues)));
            sb.AppendLine("<span aria-hidden=\"true\">&raquo;</span>");
            sb.AppendLine("</a>");
            sb.AppendLine("</li>");
            sb.AppendLine("</ul>");
            sb.AppendLine("</nav>");

            sb = (pageIndex == 0) ? sb.Replace("[pre]", "class=\"disabled\"") : sb.Replace("[pre]","");
            sb = (pageIndex == pageCount-1) ? sb.Replace("[next]", "class=\"disabled\"") : sb.Replace("[next]", "");

            return sb.ToString();
        }
    }
}