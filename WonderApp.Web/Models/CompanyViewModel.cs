using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WonderApp.Models;

namespace WonderApp.Web.Models
{
    public class CompanyViewModel
    {
        public CompanyModel CompanyModel { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
    }
}