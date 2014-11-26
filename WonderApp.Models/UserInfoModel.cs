using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class UserInfoModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }

        public virtual GenderModel Gender { get; set; }
        public virtual List<CategoryModel> MyCategories { get; set; }
        public virtual UserPreferenceModel UserPreference { get; set; }
    }
}
