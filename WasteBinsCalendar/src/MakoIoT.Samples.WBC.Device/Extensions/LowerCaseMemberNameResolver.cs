using nanoFramework.Json;

namespace MakoIoT.Samples.WBC.Device.Extensions
{
    public class LowerCaseMemberNameResolver : IMemberNameResolver
    {
        public string ResolveMemberName(string name)
        {
            return $"{name[0].ToUpper()}{name.Substring(1)}";
        }
    }
}
