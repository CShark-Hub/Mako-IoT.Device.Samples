using System;
using System.Diagnostics;
using nanoFramework.Json;

namespace MakoIoT.Samples.WBC.Device.Extensions
{
    public class IsoDateTimeJsonConverter : IJsonConverter
    {
        public object Read(object value)
        {
            DateTime o;
            string s = value.ToString();
            if (DateTime.TryParse(s, out o))
                return o;

            if (s.Length <= 19 || s[19] != '.')
            {
                if (DateTime.TryParse(s.Substring(0, 19) + ".000" + s.Substring(19), out o))
                    return o;
            }

            throw new FormatException("Unknown DateTime format");
        }
    }
}
