using System;
using System.ComponentModel;

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

        
        public double PercentageSwipesLondon { get; set; }
        public double PercentageSwipesNewYork { get; set; }
       
        
        public int TotalPasses { get; set; }
        public int TotalSaves { get; set; }
    }
}