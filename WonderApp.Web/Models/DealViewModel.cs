using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WonderApp.Models;

namespace WonderApp.Web.Models
{
    public class DealViewModel
    {
        public DealModel DealModel { get; set; }
        public IEnumerable<SelectListItem> CostRanges { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }

    }
}