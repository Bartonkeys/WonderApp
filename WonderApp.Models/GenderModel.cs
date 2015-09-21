using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WonderApp.Models
{
    public class GenderModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [DisplayName("Gender")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

    }
}
