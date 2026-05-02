using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Bug.BetterThanYesterday.Domain.Extensions
{
    public static class StringExtensions
    {
        public static string OnlyDigits(this string value)
        {
            var regex = new Regex(@"\D");
            return regex.Replace(value, string.Empty);
        }
    }
}