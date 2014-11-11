//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WonderApp.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Deal
    {
        public Deal()
        {
            this.Tags = new HashSet<Tag>();
            this.MyWonderUsers = new HashSet<AspNetUser>();
            this.MyRejectUsers = new HashSet<AspNetUser>();
            this.Images = new HashSet<Image>();
            this.Ages = new HashSet<Age>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public System.DateTime ExpiryDate { get; set; }
        public int Likes { get; set; }
        public bool Publish { get; set; }
        public Nullable<bool> Archived { get; set; }
        public string IntroDescription { get; set; }
        public Nullable<bool> Priority { get; set; }
        public int CityId { get; set; }
        public Nullable<bool> AlwaysAvailable { get; set; }
        public Nullable<int> AddressId { get; set; }
        public string Creator_User_Id { get; set; }
        public Nullable<int> Season_Id { get; set; }
        public string Phone { get; set; }
        public Nullable<int> Gender_Id { get; set; }
    
        public virtual Company Company { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual Location Location { get; set; }
        public virtual Cost Cost { get; set; }
        public virtual ICollection<AspNetUser> MyWonderUsers { get; set; }
        public virtual ICollection<AspNetUser> MyRejectUsers { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual City City { get; set; }
        public virtual Address Address { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        public virtual Season Season { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<Age> Ages { get; set; }
    }
}
