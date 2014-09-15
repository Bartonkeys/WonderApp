using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public virtual GenderModel Gender { get; set; }
        public virtual List<AspNetUserLoginModel> AspNetUserLogins { get; set; }
        public virtual List<DealModel> MyWonders { get; set; }
        public virtual List<DealModel> MyRejects { get; set; }
        public virtual List<ReminderModel> MyReminder { get; set; }
        public virtual List<CategoryModel> MyCategories { get; set; }
        public virtual List<LocationModel> Locations { get; set; }
    }
}
