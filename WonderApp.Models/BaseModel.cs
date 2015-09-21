using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WonderApp.Models
{

    public abstract class BaseModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
    }
}
