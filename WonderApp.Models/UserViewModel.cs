using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WonderApp.Models
{
    public class UserViewModel
    {
       
        public UserBasicModel UserModel { get; set;  }

        [DisplayName("Old Password")]
        public String OldPassword { get; set; }

        [DisplayName("New Password")]
        public String NewPassword { get; set; }

        [DisplayName("Admin User?")]
        public Boolean IsAdmin { get; set; }
        [DisplayName("Opt In?")]
        public Boolean OptIn { get; set; }
        [DisplayName("City")]
        public String City { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public double PercentageSwipesLondon { get; set; }

        [DisplayFormat(DataFormatString = "{0:0,0.00}", ApplyFormatInEditMode = true)]
        public double PercentageSwipesNewYork { get; set; }


        public int TotalPassesLondon { get; set; }
        public int TotalSavesLondon { get; set; }
        public int TotalPassesNewYork { get; set; }
        public int TotalSavesNewYork { get; set; }

        public int TotalPassesHomeCity { get; set; }
        public int TotalSavesHomeCity { get; set; }
    }
}