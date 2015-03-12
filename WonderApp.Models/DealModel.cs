using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Foolproof;

namespace WonderApp.Models
{
    public class DealModel
    {
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a title")]
        [JsonProperty(PropertyName = "title")]
        [MaxLength(23, ErrorMessage = "Title is too long, 23 characters or less")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter a phone number")]
        [JsonProperty(PropertyName = "phone")]
        [MaxLength(200, ErrorMessage = "Phone number is too long, 200 characters or less")]
        public string Phone { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description")]
        [JsonProperty(PropertyName = "description")]
        [MaxLength(140, ErrorMessage = "Intro is too long, 140 characters or less")]
        public string Description { get; set; }

        [Display(Name = "Introduction")]
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Please enter a description for the popup introduction")]
        [JsonProperty(PropertyName = "introDescription")]
        [MaxLength(58, ErrorMessage="Intro is too long, 58 characters or less")]
        public string IntroDescription { get; set; }

        [Required(ErrorMessage = "Please enter a url")]
        [JsonProperty(PropertyName = "url")]
        [Url]
        public string Url { get; set; }

        [JsonProperty(PropertyName = "alwaysAvailable")]
        [Display(Name = "Always Available?")]
        public Boolean AlwaysAvailable { get; set; }

        [DisplayName("Expiry Date")]
        [DisplayFormat(DataFormatString = "{0:ddd d MMMM yyyy}", ApplyFormatInEditMode = true)]
        [JsonProperty(PropertyName = "expiryDate")]
        [RequiredIfFalse("AlwaysAvailable", ErrorMessage = "Please select Expiry Date if Always Available not checked")]
        public String ExpiryDate { get; set; }

        [JsonProperty(PropertyName = "likes")]
        public int Likes { get; set; }

        [JsonProperty(PropertyName = "company")]
        public CompanyModel Company { get; set; }

        [JsonIgnore]
        public bool Publish { get; set; }

        [JsonProperty(PropertyName = "archived")]
        [JsonIgnore]
        public bool Archived { get; set; }

        [JsonProperty(PropertyName = "expired")]
        [Display(Name = "Expire")]
        public bool Expired { get; set; }

        [JsonProperty(PropertyName = "priority")]
        public bool Priority { get; set; }

        [Display(Name = "Tags")]
        [JsonProperty(PropertyName = "tags")]
        public virtual List<TagModel> Tags { get; set; }

        [JsonProperty(PropertyName = "location")]
        public virtual LocationModel Location { get; set; }

        [JsonProperty(PropertyName = "range")]
        public virtual CostModel Cost { get; set; }

        [JsonProperty(PropertyName = "category")]
        public virtual CategoryModel Category { get; set; }

        [JsonProperty(PropertyName = "images")]
        public virtual List<ImageModel> Images { get; set; }

        [JsonProperty(PropertyName = "city")]
        public virtual CityModel City { get; set; }

        [JsonProperty(PropertyName = "address")]
        public virtual AddressModel Address { get; set; }

        [JsonProperty(PropertyName = "creator")]
        [JsonIgnore]
        public virtual UserModel Creator { get; set; }

        [JsonProperty(PropertyName = "season")]
        public virtual SeasonModel Season { get; set; }

        [JsonProperty(PropertyName = "gender")]
        public virtual GenderModel Gender { get; set; }

        [Display(Name = "Ages")]
        [JsonProperty(PropertyName = "ages")]
        public virtual List<AgeModel> Ages { get; set; }

    }
}
