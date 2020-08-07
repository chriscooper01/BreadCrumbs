using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreadCrumb.Pages.Extended
{
    public class BreadCrumbsExtended : PageModel
    {
        private List<string> referers;
        private List<BreadCrumbDataClass> breadCrumbs;
        private string currentPath;
        private string currentQuery;
        public string BreadCrumb;
        public void SetBreadCrumbs()
        {
            if (HttpContext == null)
                return;

                currentQuery = String.Empty;
            currentPath = String.Empty;
            referers = this.Request.Headers["Referer"].ToList(); ;
            if (this.Request.Path.HasValue)
                currentPath = this.Request.Path.Value.Replace("/", String.Empty);
            if (this.Request.QueryString.HasValue)
                currentQuery = this.Request.QueryString.Value;

            getCurrentBreadCrumbs();

            checkGoingBack();
            setBreadCrumbs();

            setCurrent();
            setSession();

            BreadCrumb = BreadCrumbsNavigationExtended.SetNavString(breadCrumbs);
            //Dispos
            currentQuery = String.Empty;
            currentPath = String.Empty;
            referers = null;
            breadCrumbs = null;
        }

        public void WipeBreadCrumbs()
        {
            if (HttpContext == null)
                return;

                getCurrentBreadCrumbs();
            breadCrumbs = new List<BreadCrumbDataClass>();
            setSession();
        }
        private void getCurrentBreadCrumbs()
        {
            

            var sessionString = HttpContext.Session.GetString("BreadCrumbs");
            breadCrumbs = new List<BreadCrumbDataClass>();
            if (String.IsNullOrWhiteSpace(sessionString) || sessionString.Equals("[]"))
                return;

            breadCrumbs = (List<BreadCrumbDataClass>)JsonStringHelper.GetObject(sessionString, breadCrumbs.GetType());

        }


        private void setBreadCrumbs()
        {
            if (referers.Count > 0)
            {
                var urlValue = referers[0].ToString();
                var urlElements = urlValue.Split("/").ToList();
                var hostLocation = urlElements.IndexOf(this.Request.Host.Value);
                if (urlElements.Count() > hostLocation)
                {
                    var razorPageURL = urlElements[hostLocation + 1];
                    var razorPage = razorPageURL.Split("?");
                    if (razorPage.Count() > 0)
                    {
                        var data = new BreadCrumbDataClass();
                        data.Key = razorPage[0].ToString();
                        data.Display = data.Key;
                        if (String.IsNullOrWhiteSpace(data.Key))
                        {//This means you have come from the home page.
                            data.Key = "Index";
                            data.Display = "Home";
                        }
                        //Make sure I'm not adding the page back in I have removed
                        if (crumbRemoved.FirstOrDefault(x => x.Equals(data.Key)) != null)
                            return;

                        if (setCurrentAsDisabled(data.Key))
                            return;//Do not set if already exists


                        if (razorPage.Count().Equals(2))
                        {
                            data.Parameters = setParameters(razorPage[1]);
                        }
                        breadCrumbs.Add(data);
                    }

                }
            }
        }

        private List<string> crumbRemoved;

        /// <summary>
        /// This will check that the Current location is going back wards within the current bread crumbs and if 
        /// so it will remove it and all after it
        /// </summary>
        private void checkGoingBack()
        {
            crumbRemoved = new List<string>();
            var currentValue = breadCrumbs.FirstOrDefault(x => x.Key.Equals(currentPath) && x.Current.Equals(false));
            if (currentValue != null)
            {//This means I need to remove myself and anything after me.
                var currentLocation = breadCrumbs.IndexOf(currentValue);
                if (currentLocation < breadCrumbs.Count)
                {

                    while (currentLocation < breadCrumbs.Count)
                    {
                        crumbRemoved.Add(breadCrumbs[currentLocation].Key);
                        breadCrumbs.RemoveAt(currentLocation);
                    }
                }
            }
        }

        private void setCurrent()
        {
            if (String.IsNullOrWhiteSpace(currentPath))
                return;

            var currentValue = breadCrumbs.FirstOrDefault(x => x.Key.Equals(currentPath) && x.Current.Equals(true));
            if (currentValue == null)
            {
                //There can only be one!!
                var currentCurrnet = breadCrumbs.FirstOrDefault(x => x.Current.Equals(true));
                if (currentCurrnet != null)
                    currentCurrnet.Current = false;


                var data = new BreadCrumbDataClass();
                data.Key = currentPath;
                data.Display = currentPath;
                data.Parameters = setParameters(currentQuery);
                data.Current = true;

                breadCrumbs.Add(data);




            }



        }

        /// <summary>
        /// This will check if the prev page is already been set as current and if so then it 
        /// set current as False and then can be uses as a linked crumb
        /// </summary>
        /// <param name="key"></param>
        private bool setCurrentAsDisabled(string key)
        {
            if (String.IsNullOrWhiteSpace(key))
                return true;

            var currentValue = breadCrumbs.FirstOrDefault(x => x.Key.Equals(key));
            if (currentValue != null)
            {

                if (currentPath.Equals(key))
                    return true;

                if (currentValue.Current)
                    currentValue.Current = false;

                return true;
            }

            return false;
        }

        private void setSession()
        {
            HttpContext.Session.Set("BreadCrumbs", System.Text.Encoding.UTF8.GetBytes(JsonStringHelper.GetString(breadCrumbs)));
        }


        private List<BreadCrumbDataClass> setParameters(string query)
        {
            if (String.IsNullOrWhiteSpace(query))
                return new List<BreadCrumbDataClass>();

            if (query.StartsWith("?"))
                query = query.Remove(0, 1);

            var parameters = query.Split("&").Select(s => new BreadCrumbDataClass()
            {
                Key = s.Split("=")[0].ToString(),
                Value = s.Split("=")[1].ToString()
            }).ToList();

            return parameters;
        }



    }
}
