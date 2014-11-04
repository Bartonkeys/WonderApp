using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WonderApp.Models;

namespace WonderApp.Web.Models
{
    public class DealViewModel
    {
        public DealModel DealModel { get; set; }

        [Required(ErrorMessage = "Please add at least 1 tag")]
        [DisplayName("Tags")]
        public string TagString { get; set; }
        public IEnumerable<SelectListItem> CostRanges { get; set; }
        public IEnumerable<SelectListItem> Categories { get; set; }
        public IEnumerable<SelectListItem> Companies { get; set; }
        public IEnumerable<SelectListItem> Locations { get; set; }
        public IEnumerable<SelectListItem> Cities { get; set; }
        public IEnumerable<SelectListItem> Seasons { get; set; }

    }
}