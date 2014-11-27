using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WonderApp.Models;

namespace WonderApp.Web.Models
{
    public class SeasonExpiryViewModel
    {
        public SeasonModel Season { get; set; }
        public bool Expired { get; set; }
    }
}