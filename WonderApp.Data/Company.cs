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
    
    public partial class Company
    {
        public Company()
        {
            this.Deals = new HashSet<Deal>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Phone { get; set; }
        public int CityId { get; set; }
    
        public virtual ICollection<Deal> Deals { get; set; }
        public virtual Country Country { get; set; }
        public virtual City City { get; set; }
    }
}
