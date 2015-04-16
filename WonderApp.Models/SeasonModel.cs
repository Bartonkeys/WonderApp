using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class SeasonModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [DisplayName("Season")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        //[JsonIgnore]
        //public virtual List<DealModel> Deals { get; set; }
    }
}
