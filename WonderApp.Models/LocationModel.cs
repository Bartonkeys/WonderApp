using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Newtonsoft.Json;

namespace WonderApp.Models
{
    public class LocationModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select a location")]
        [DisplayName("PostCode")]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "latitude")]
        public double? Latitude { get; set; }

        [JsonIgnore]
        public virtual List<DealModel> Deals { get; set; }
    }
}
