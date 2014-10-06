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

        [Display(Name = "Introduction")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description for the popup introduction")]
        public string IntroDescription { get; set; }

        [Required(ErrorMessage = "Please enter a url")]
        public string Url { get; set; }

        [DisplayName("Expiry Date")]
        [Required(ErrorMessage="Please enter date")]
        [DisplayFormat(DataFormatString = "{0:ddd d MMMM yyyy}", ApplyFormatInEditMode = true)]
        public String ExpiryDate { get; set; }

        public int Likes { get; set; }

        public CompanyModel Company { get; set; }

        public bool Publish { get; set; }
        public bool Archived { get; set; }

        [Display(Name = "Tags")]
        public virtual List<TagModel> Tags { get; set; }

        public virtual LocationModel Location { get; set; }

        [JsonProperty(PropertyName = "cost")]
        public virtual CostModel Cost { get; set; }

        public virtual CategoryModel Category { get; set; }

        public virtual List<ImageModel> Images { get; set; }
    }
}
