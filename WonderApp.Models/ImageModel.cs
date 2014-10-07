using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class ImageModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Display(Name="Image")]
        [JsonProperty(PropertyName = "url")]
        public string url { get; set; }

        [JsonIgnore]
        public virtual DealModel Deal { get; set; }

        [JsonIgnore]
        public virtual DeviceModel Device { get; set; }
    }
}
