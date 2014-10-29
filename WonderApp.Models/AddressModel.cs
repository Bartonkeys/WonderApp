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

        [DisplayName("Address Line 1")]
        [Required(ErrorMessage = "Please enter an address")]
        [JsonProperty(PropertyName = "addressLine1")]
        public string AddressLine1 { get; set; }

        [DisplayName("Address Line 2")]
        [JsonProperty(PropertyName = "addressLine2")]
        public string AddressLine2 { get; set; }

        [JsonProperty(PropertyName = "postCode")]
        public string PostCode { get; set; }

    }
}
