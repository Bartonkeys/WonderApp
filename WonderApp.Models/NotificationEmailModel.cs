using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WonderApp.Models
{
    public class NotificationEmailModel
    {

        
        [JsonProperty(PropertyName = "id")]
        public int Id { get; set; }

        [JsonProperty(PropertyName = "created")]
        public DateTime Created { get; set; }

        [JsonProperty(PropertyName = "sent")]
        public DateTime Sent { get; set; }

        [JsonProperty(PropertyName = "recipients")]
        public string Recipients { get; set; }


    }
}

