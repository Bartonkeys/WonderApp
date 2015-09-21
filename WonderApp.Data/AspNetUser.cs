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
    
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            this.AspNetUserClaims = new HashSet<AspNetUserClaim>();
            this.AspNetUserLogins = new HashSet<AspNetUserLogin>();
            this.Roles = new HashSet<AspNetRole>();
            this.MyWonders = new HashSet<Deal>();
            this.MyRejects = new HashSet<Deal>();
            this.Locations = new HashSet<Location>();
            this.Categories = new HashSet<Category>();
            this.Deals = new HashSet<Deal>();
        }
    
        public string Id { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public Nullable<System.DateTime> LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Forename { get; set; }
        public string Surname { get; set; }
        public string AppUserName { get; set; }
        public Nullable<int> CityId { get; set; }
        public Nullable<bool> ShowTutorial { get; set; }
        public Nullable<bool> ShowInfoRequest { get; set; }
        public Nullable<int> YearOfBirth { get; set; }
    
        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetRole> Roles { get; set; }
        public virtual Gender Gender { get; set; }
        public virtual ICollection<Deal> MyWonders { get; set; }
        public virtual ICollection<Deal> MyRejects { get; set; }
        public virtual ICollection<Location> Locations { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
        public virtual UserPreference UserPreference { get; set; }
    }
}
