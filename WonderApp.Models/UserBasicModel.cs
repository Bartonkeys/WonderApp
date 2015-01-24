using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class UserBasicModel
    {
        public string Id { get; set; }
        public string Email { get; set; }

        [DisplayName("User Name")]
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
    }
}
