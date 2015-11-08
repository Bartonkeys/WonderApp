using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class RadiusModel: WonderModel
    {
        [JsonProperty(PropertyName = "radius")]
        public double Radius { get; set; }
    }
}
