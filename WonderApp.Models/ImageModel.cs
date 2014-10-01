using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        public string url { get; set; }

        public virtual DealModel Deal { get; set; }
        public virtual DeviceModel Device { get; set; }
    }
}
