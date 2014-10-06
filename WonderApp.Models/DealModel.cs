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

        [Required(ErrorMessage = "Please enter a title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description")]
        public string Description { get; set; }

        [JsonIgnore]
        [Display(Name = "Introduction Description")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description for the popup introduction")]
        public string IntroDescription { get; set; }

        [Required(ErrorMessage = "Please enter a url")]
        public string Url { get; set; }

        [JsonIgnore]
        [DisplayName("Expiry Date")]
        [Required(ErrorMessage="Please enter date")]
        [DisplayFormat(DataFormatString = "{0:ddd d MMMM yyyy}", ApplyFormatInEditMode = true)]
        public String ExpiryDate { get; set; }

        public int Likes { get; set; }
        [JsonIgnore]
        public CompanyModel Company { get; set; }
        [JsonIgnore]
        public bool Publish { get; set; }
        [JsonIgnore]
        public bool Archived { get; set; }
        [JsonIgnore]
        public virtual List<TagModel> Tags { get; set; }
        [JsonIgnore]
        public virtual LocationModel Location { get; set; }
        [JsonIgnore]
        [JsonProperty(PropertyName = "cost")]
        public virtual CostModel Cost { get; set; }
        [JsonIgnore]
        public virtual CategoryModel Category { get; set; }
        [JsonIgnore]
        public virtual List<ImageModel> Images { get; set; }
    }
}
