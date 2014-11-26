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
    public class AddressModel
    {
       
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter an address")]
        [JsonProperty(PropertyName = "addressLine1")]
        [MaxLength(15)]
        public string AddressLine1 { get; set; }

        [DisplayName("Street")]
        [JsonProperty(PropertyName = "addressLine2")]
        [MaxLength(15)]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "postCode")]
        public string PostCode { get; set; }

    }
}
