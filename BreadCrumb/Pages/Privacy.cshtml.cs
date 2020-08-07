using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BreadCrumb.Pages
{
    public class PrivacyModel : Extended.BreadCrumbsExtended
    {
        private readonly ILogger<PrivacyModel> _logger;

        
        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;

        }

        public void OnGet()
        {
            SetBreadCrumbs();
        }
    }
}
