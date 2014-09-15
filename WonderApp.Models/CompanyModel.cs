using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class CompanyModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Phone { get; set; }

        public List<DealModel> Deals { get; set; }
        public CountryModel Country { get; set; }
    }
}
