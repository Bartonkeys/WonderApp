using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WonderApp.Web.Models
{
    public class ImageViewModel
    {
        public int DealId { get; set; }
        public HttpPostedFileBase Image { get; set; }
    }
}