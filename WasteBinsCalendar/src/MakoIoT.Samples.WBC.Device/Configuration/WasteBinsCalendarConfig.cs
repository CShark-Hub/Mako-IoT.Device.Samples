using System.Collections;

namespace MakoIoT.Samples.WBC.Device.Configuration
{
    public class WasteBinsCalendarConfig
    {
        public string CalendarUrl { get; set; }
        public string ServiceCertificate { get; set; }
        public string Timezone => TimezoneString?.Split(';')[0];
        public string TimezoneString { get; set; }
        public Hashtable BinsNames { get; set; }
        public static string SectionName => "WasteBinsCalendar";
    }
}
