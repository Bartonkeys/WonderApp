using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WonderApp.Models
{
    public class DealSummaryModel
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<TagModel> Tags { get; set; }

        public string IntroDescription { get; set; }

        public int Likes { get; set; }

        public bool Publish { get; set; }

        public bool Archived { get; set; }

        public bool Priority { get; set; }

        public bool Broadcast { get; set; }

        public bool Expired { get; set; }

        public CompanyModel Company { get; set; }

        public virtual CityModel City { get; set; }

        public virtual UserModel Creator { get; set; }

        public virtual SeasonModel Season { get; set; }
    }
}
