using System.Collections;

namespace MakoIoT.Device.LocalConfiguration.Extensions
{
    public static class HashtableExtension
    {
        public static void AddOrUpdate(this Hashtable t, object key, object value)
        {
            if (t.Contains(key))
                t[key] = value;
            else
                t.Add(key, value);
        }
    }
}
