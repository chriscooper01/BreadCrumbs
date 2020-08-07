using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BreadCrumb.Pages.Extended
{
    public class BreadCrumbDataClass
    {
        public string Key { get; set; }
        public string Value { get; set; }
        public string Display { get; set; }
        public bool Current { get; set; }
        public List<BreadCrumbDataClass> Parameters { get; set; }
    }
}
