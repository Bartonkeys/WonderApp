using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WonderApp.Models.Extensions
{
    public static class DateTimeExtensions
    {
        public static string MapToString(this DateTime dateTime)
        {
            return dateTime.ToString("ddd dd MMMM yyyy");
        }
    }
}