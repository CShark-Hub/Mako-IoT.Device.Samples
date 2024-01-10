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

        public static bool TryAdd(this Hashtable t, object key, object value)
        {
            if (t.Contains(key))
                return false;
            t.Add(key, value);
            return true;
        }

        public static object GetValueOrDefault(this Hashtable t, object key, object defaultValue = null)
        {
            return t.Contains(key) ? t[key] : defaultValue;
        }

        public static Hashtable Reverse(this Hashtable t)
        {
            var r = new Hashtable(t.Count);
            foreach (var key in t.Keys)
            {
                r.TryAdd(t[key], key);
            }
            return r;
        }
    }
}
