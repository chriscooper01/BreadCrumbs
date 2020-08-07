using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadCrumb.Pages.Extended
{
    public class BreadCrumbsNavigationExtended
    {

        public static string SetNavString(List<BreadCrumbDataClass> crumbs)
        {
            if (crumbs == null || crumbs.Count.Equals(0))
                return String.Empty;


            var nav = new StringBuilder();
            nav.AppendLine("<nav aria-label=\"breadcrumb\">");
            nav.AppendLine("<ol class=\"breadcrumb\">");

            foreach (var crumb in crumbs)
                nav.AppendLine(setLinkItem(crumb));

            nav.AppendLine("</ol>");
            nav.AppendLine("</nav>");
            return nav.ToString();
        }


        private static string setLinkItem(BreadCrumbDataClass crumb)
        {
            var item = new StringBuilder();
            if (!crumb.Current)
            {
                item.AppendLine("<li class=\"breadcrumb-item \">");
                item.AppendLine(String.Format("<a href=\"{0}{1}\" >{2}</a>", crumb.Key, setLinkItemParemeters(crumb), crumb.Display));
            }
            else
            {
                item.AppendLine("<li class=\"breadcrumb-item active\" aria-current=\"page\">");
                item.AppendLine(crumb.Display);
            }



            item.AppendLine("</li>");
            return item.ToString();
        }

        private static string setLinkItemParemeters(BreadCrumbDataClass crumb)
        {
            if (crumb.Parameters == null || crumb.Parameters.Count.Equals(0))
                return String.Empty;

            var parameter = new List<string>();
            foreach (var p in crumb.Parameters)
            {
                parameter.Add(String.Format("{0}={1}", p.Key, p.Value));
            }

            return "?" + String.Join("&", parameter.ToArray());
        }
    }
}