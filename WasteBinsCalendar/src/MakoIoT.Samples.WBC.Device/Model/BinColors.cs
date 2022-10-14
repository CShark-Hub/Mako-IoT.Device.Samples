using MakoIoT.Device.Displays.Led;

namespace MakoIoT.Samples.WBC.Device.Model
{
    public static class BinColors
    {
        public static Color Black = new Color(255, 255, 255);
        public static Color Brown = new Color(255, 60, 0);
        public static Color Yellow = new Color(255, 255, 0);
        public static Color Green = new Color(0, 255, 0);
        public static Color Blue = new Color(0, 0, 255);
        public static Color Red = new Color(255, 0, 0);

        public static Color FromString(string s)
        {
            return s switch
            {
                "Black" => Black,
                "Brown" => Brown,
                "Yellow" => Yellow,
                "Green" => Green,
                "Blue" => Blue,
                "Red" => Red,
                _ => new Color(0, 0, 0)
            };
        }
    }
}
