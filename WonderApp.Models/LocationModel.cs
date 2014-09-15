using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace WonderApp.Models
{
    public class LocationModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Longitude { get; set; }
        public decimal Latitude { get; set; }

        public virtual List<DealModel> Deals { get; set; }
    }
}
