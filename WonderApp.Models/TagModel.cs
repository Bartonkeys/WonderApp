using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class TagModel
    {
        [JsonProperty(PropertyName="id")]
        public int Id { get; set; }

        [MaxLength(100)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        //[JsonIgnore]
        //public virtual List<DealModel> Deals { get; set; }
    }
}
