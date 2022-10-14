using System.Text;

namespace MakoIoT.Samples.WBC.Device.Extensions
{
    public static class StringExtension
    {
        public static string Replace(this string s, string oldString, string newString)
        {
            var builder = new StringBuilder(s);
            builder.Replace(oldString, newString);
            return builder.ToString();
        }

        public static string EscapeJson(this string s)
        {
            return s.Replace("\"", "&&amp;");
        }

        public static string UnEscapeJson(this string s)
        {
            return s.Replace("&&amp;", "\"");
        }

        public static string EscapeForInterpolation(this string s)
        {
            var builder = new StringBuilder(s);
            builder.Replace("{", "{{").Replace("}", "}}");
            return builder.ToString();
        }
    }
}
