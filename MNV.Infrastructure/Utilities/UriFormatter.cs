using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Infrastructure.Utilities
{
    public static class UriFormatter
    {
        public static string RemoveInvalidUriCharaters(this string uriString)
        {
            string formattedUriString = uriString
                .Replace("-", "")
                .Replace(" ", "")
                .Replace(".", "")
                .Replace(";", "")
                .Replace("/", "")
                .Replace("?", "")
                .Replace(":", "")
                .Replace("@", "")
                .Replace("&", "")
                .Replace("=", "")
                .Replace("+", "")
                .Replace("$", "")
                .Replace(",", "");

            return formattedUriString;
        }
    }
}
