using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WonderApp.Data;

namespace WonderApp.Web.Models
{
    public class DealEditModel: DealViewModel
    {
        [Display(Name = "Upload New Image")]
        public HttpPostedFileBase Image { get; set; }
    }
}