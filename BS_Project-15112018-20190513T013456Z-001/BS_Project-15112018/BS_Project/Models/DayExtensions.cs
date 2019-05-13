using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace BS_Project.Models
{
    public static class DayExtensions
    {
        public const string VN_CULTURE = "vi-VN";

        public static string GetJstDateTime(this DateTime current, string format)
        {
            CultureInfo cultureInfo = new CultureInfo(VN_CULTURE);
            return current.ToString(format, cultureInfo); ;
        }
    }
}