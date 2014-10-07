using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class CategoryModel
    {
        [Required(ErrorMessage = "Please select a category")]
        [DisplayName("Category")]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonIgnore]
        public virtual List<UserModel> Users { get; set; }
        [JsonIgnore]
        public virtual List<DealModel> Deals { get; set; }
    }
}
