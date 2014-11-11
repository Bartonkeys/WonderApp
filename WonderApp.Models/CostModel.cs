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
    public class CostModel
    {
        [Required(ErrorMessage = "Please select a cost range")]
        [DisplayName("Cost Range")]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Range { get; set; }
    }
}
