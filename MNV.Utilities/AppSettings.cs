using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MNV.Utilities
{
    public static class AppSettings
    {
        public static T Setting<T>(string key)
        {
            string value = ConfigurationManager.AppSettings[key];

            if (value == null)
                throw new Exception(String.Format("Could not find setting '{0}',", key));

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}
