using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class TagSearchModel
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }

        [JsonProperty(PropertyName = "cityId")]
        public int CityId { get; set; }

        [MaxLength(100)]
        [JsonProperty(PropertyName = "name")]
        public string TagName { get; set; }
    }
}
