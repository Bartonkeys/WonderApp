using System.Collections.Generic;

namespace WonderApp.Models
{
    public class EmailTemplateViewModel
    {
        public List<DealModel> Wonders { get; set; }
        public UserModel User { get; set; }
        public string UrlString { get; set; }
        public string UnsubscribeLink { get; set; }

    }
}