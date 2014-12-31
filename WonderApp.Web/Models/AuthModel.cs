﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace WonderApp.Web.Models
{
    public class AuthModel
    {
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }

    }
}