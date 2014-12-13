using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Web;
using WonderApp.Models;

namespace WonderApp.Web.Models
{
    public class UserViewModel
    {
        public UserModel UserModel { get; set;  }

        [DisplayName("Old Password")]
        public String OldPassword { get; set; }

        [DisplayName("New Password")]
        public String NewPassword { get; set; }

        [DisplayName("Admin User?")]
        public Boolean IsAdmin { get; set; }




    }
}