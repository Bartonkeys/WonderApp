using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class DeviceModel
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public virtual List<ImageModel> Images { get; set; }
    }
}
