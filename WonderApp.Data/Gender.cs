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
    
    public partial class Gender
    {
        public Gender()
        {
            this.AspNetUsers = new HashSet<AspNetUser>();
            this.Deals = new HashSet<Deal>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
    
        public virtual ICollection<AspNetUser> AspNetUsers { get; set; }
        public virtual ICollection<Deal> Deals { get; set; }
    }
}
