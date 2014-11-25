using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Elmah.ContentSyndication;

namespace WonderApp.Web.Models
{
    public class DealCreateModel: DealViewModel
    {
        //[Required(ErrorMessage = "Please upload an image")]
        [Display(Name = "Upload Image")]
        public HttpPostedFileBase Image { get; set; }
        public Data.Image UploadedImage { get; set; }
    }
}