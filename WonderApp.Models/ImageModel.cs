using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WonderApp.Models
{
    public class ImageModel
    {
        public int Id { get; set; }
        [Display(Name="Image")]
        public string url { get; set; }

        public virtual DealModel Deal { get; set; }
        public virtual DeviceModel Device { get; set; }
    }
}
