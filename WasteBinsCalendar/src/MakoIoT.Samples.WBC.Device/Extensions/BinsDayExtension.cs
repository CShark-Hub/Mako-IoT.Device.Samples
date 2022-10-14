using System;
using MakoIoT.Device.Displays.Led;
using MakoIoT.Samples.WBC.Device.Model;

namespace MakoIoT.Samples.WBC.Device.Extensions
{
    public static class BinsDayExtension
    {
        public static Color[] ToColorsArray(this BinsDay binsDay)
        {
            var colors = new Color[binsDay.Bins.Length];
            for (int i = 0; i < binsDay.Bins.Length; i++)
            {
                colors[i] = BinColors.FromString(binsDay.Bins[i]);
            }
            return colors;
        }

        public static BinsDay FindByDate(this BinsDay[] days, DateTime date)
        {
            for (int i = 0; i < days.Length; i++)
            {
                if (days[i].Day.Date.Equals(date.Date))
                    return days[i];
            }
            return null;
        }
    }
}
