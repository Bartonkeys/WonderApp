using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WonderApp.Web.Models
{
    public class CostViewModel
    {
        [Display(Name = "Cost Range")]
        public int Id { get; set; }
        public IEnumerable<SelectListItem> CostRanges { get; set; }
    }
}