using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bug.BetterThanYesterday.Domain.Extensions
{
    public static class ListExtensions
    {
        public static string JoinThis(this IEnumerable<string> list, string separator = ", ")
        {
            return string.Join(separator, list);
        }
    }
}