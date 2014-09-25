using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WonderApp.Models
{
    public class DealModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        public string Url { get; set; }

        [DisplayName("Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime? ExpiryDate { get; set; }

        public int Likes { get; set; }

        public CompanyModel Company { get; set; }
        public virtual List<TagModel> Tags { get; set; }

        public virtual LocationModel Location { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public virtual CostModel Cost { get; set; }

        public virtual CategoryModel Category { get; set; }
    }
}
