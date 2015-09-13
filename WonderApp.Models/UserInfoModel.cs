using Newtonsoft.Json;
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
        public string AppUserName { get; set; }
        public int? CityId { get; set; }
        public bool? ShowTutorial { get; set; }
        public bool? ShowInfoRequest { get; set; }
        public int? YearOfBirth { get; set; }
        public virtual GenderModel Gender { get; set; }
        public virtual List<CategoryModel> MyCategories { get; set; }
        public virtual UserPreferenceModel UserPreference { get; set; }
    }

    public class UserCategoryModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "myCategories")]
        public virtual List<CategoryModel> MyCategories { get; set; }
    }

    public class UserEmailModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "emailMyWonders")]
        public bool EmailMyWonders { get; set; }

        [JsonProperty(PropertyName = "reminderId")]
        public int ReminderId { get; set; }
    }

    public class UserGenderModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public virtual GenderModel Gender { get; set; }
    }
}
