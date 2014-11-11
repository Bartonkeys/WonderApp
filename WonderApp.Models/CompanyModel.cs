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
    public class CompanyModel
    {
        [Required(ErrorMessage = "Please select a company")]
        [DisplayName("company")]
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "address")]
        [JsonIgnore]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "postcode")]
        [JsonIgnore]
        public string PostCode { get; set; }

        [JsonProperty(PropertyName = "county")]
        [JsonIgnore]
        public string County { get; set; }

        [JsonProperty(PropertyName = "phone")]
        [JsonIgnore]
        public string Phone { get; set; }

        [JsonIgnore]
        public List<DealModel> Deals { get; set; }
        [JsonIgnore]
        public CountryModel Country { get; set; }
        [JsonIgnore]
        public CityModel City { get; set; }
    }
}
