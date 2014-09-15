using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class CategoryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<UserModel> Users { get; set; }
        public virtual List<DealModel> Deals { get; set; }
    }
}
