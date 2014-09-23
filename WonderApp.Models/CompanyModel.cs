using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class CompanyModel
    {
        [Required(ErrorMessage = "Please select a company")]
        [DisplayName("Company")]
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
