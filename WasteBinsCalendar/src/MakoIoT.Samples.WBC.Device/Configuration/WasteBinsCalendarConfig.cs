using System.Collections;
using MakoIoT.Device.Services.Configuration.Metadata.Attributes;

namespace MakoIoT.Samples.WBC.Device.Configuration
{
    [SectionMetadata("Waste Collection Calendar")]
    public class WasteBinsCalendarConfig
    {
        [ParameterMetadata("Calendar URL (iCal)")]
        public string CalendarUrl { get; set; }
        [ParameterMetadata("HTTPS Certificate (required if URL is https://)", type:"text")]
        public string ServiceCertificate { get; set; }
        [ParameterMetadata("Time zone", type:"timezone")]
        public string Timezone { get; set; }
        [ParameterMetadata("Calendar event text to bin colour mapping", type:"text")]
        public Hashtable BinsNames { get; set; }
        public static string SectionName => "WasteBinsCalendar";
    }
}
