using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace WonderApp.Models
{
    public class DealModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Display(Name = "Introduction")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description for the popup introduction")]
        [JsonProperty(PropertyName = "introDescription")]
        public string IntroDescription { get; set; }

        [Required(ErrorMessage = "Please enter a url")]
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        [DisplayName("Expiry Date")]
        [Required(ErrorMessage="Please enter date")]
        [DisplayFormat(DataFormatString = "{0:ddd d MMMM yyyy}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "expiryDate")]
        public String ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "company")]
        public CompanyModel Company { get; set; }

        [JsonIgnore]
        public bool Publish { get; set; }

        [JsonProperty(PropertyName = "archived")]
        public bool Archived { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }

        [Display(Name = "Tags")]
        [JsonProperty(PropertyName = "tags")]
        public virtual List<TagModel> Tags { get; set; }

        [JsonProperty(PropertyName = "location")]
        public virtual LocationModel Location { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public virtual CostModel Cost { get; set; }

        [JsonProperty(PropertyName = "category")]
        public virtual CategoryModel Category { get; set; }

        [JsonProperty(PropertyName = "images")]
        public virtual List<ImageModel> Images { get; set; }

        [JsonProperty(PropertyName = "city")]
        public virtual CityModel City { get; set; }
    }
}
