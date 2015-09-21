using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WonderApp.Web.InfaStructure
{
    public class ClientMessage
    {
        public static string Success = "success";
        public static string Info = "info";
        public static string Warning = "warning";
        public static string Error = "danger";

        public string Type { get; set; }

        public string Text { get; set; }
    }
}