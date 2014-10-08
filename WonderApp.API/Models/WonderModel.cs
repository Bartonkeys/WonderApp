﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WonderApp.Models
{
    public class WonderModel
    {
        [JsonProperty(PropertyName="latitude")]
        public double? Latitude { get; set; }

        [JsonProperty(PropertyName = "longitude")]
        public double? Longitude { get; set; }

        [JsonProperty(PropertyName = "radiusInMiles")]
        public int? RadiusInMiles { get; set; }

        [JsonProperty(PropertyName = "maxWonders")]
        public int? MaxWonders { get; set; }
    }
}