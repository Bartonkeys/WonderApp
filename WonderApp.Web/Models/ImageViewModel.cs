using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WonderApp.Web.Models
{
    public class ImageViewModel
    {
        public int DealId { get; set; }

        [Required(ErrorMessage="Please upload an image")]
        [Display(Name="Upload Image")]
        public HttpPostedFileBase Image { get; set; }
    }
}