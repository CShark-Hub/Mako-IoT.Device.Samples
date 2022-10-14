using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using nanoFramework.TestFramework;
using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Model;
using MakoIoT.Samples.WBC.Device.Services;
using MakoIoT.Samples.WBC.Device.Tests.Mocks;
using nanoFramework.Logging.Debug;

namespace MakoIoT.Samples.WBC.Device.Tests
{
    [TestClass]
    public class BinsScheduleServiceTest
    {
        [TestMethod]
        public void GetBinsFromSummary_should_return_list_of_bins()
        {
            string input = "BIO, szkło - Address City etc";

            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig
                    {
                        BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        }
                    }
                }, new MockNetworkProvider(), new DebugLogger("debug"),
                new MockDateTimeProvider(),
                new MockDisplayController());

            var output = sut.GetBinsFromSummary(input);

            Assert.Equal("Brown", output[0]);
            Assert.Equal("Green", output[1]);
            Assert.Equal(2, output.Length);
        }

        [TestMethod]
        public void ParseContent_given_VEVENT_mulitple_bins_should_get_all_bins_in_the_day()
        {
            var dateProvider = new MockDateTimeProvider { UtcNow = new DateTime(2022, 10, 4) };

            string input = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:kwywoz-rules
CALSCALE:GREGORIAN
X-WR-CALNAME:KiedyWywóz
X-APPLE-LANGUAGE:pl
X-APPLE-REGION:PL
BEGIN:VEVENT
DTSTAMP:20221004T113200Z
UID:9ca13e760c8f50b36129ecc5af1fd97d
DTSTART;VALUE=DATE:20221004
CLASS:PUBLIC
SUMMARY;LANGUAGE=pl:zmieszane, BIO, papier - Wrocław
TRANSP:TRANSPARENT
CATEGORIES:KiedyWywoz, Odbior odpadow
DESCRIPTION:KiedyWywoz przypomina: odbior frakcji zmieszane w lokalizacji Wrocław
END:VEVENT
END:VCALENDAR";

            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig
                    {
                        BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        }
                    }
                }, new MockNetworkProvider(), new DebugLogger("debug"),
                dateProvider,
                new MockDisplayController());

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var result = sut.ParseContent(stream);

            Assert.Equal(3, result.Days[0].Bins.Length);
            Assert.Equal("Brown", result.Days[0].Bins[0]);
            Assert.Equal("Black", result.Days[0].Bins[1]);
            Assert.Equal("Blue", result.Days[0].Bins[2]);
        }

        [TestMethod]
        public void ParseContent_given_multiple_VEVENT_same_date_should_get_all_bins_in_the_day()
        {
            var dateProvider = new MockDateTimeProvider { UtcNow = new DateTime(2022, 10, 4) };

            string input = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:kwywoz-rules
CALSCALE:GREGORIAN
X-WR-CALNAME:KiedyWywóz
X-APPLE-LANGUAGE:pl
X-APPLE-REGION:PL
BEGIN:VEVENT
DTSTAMP:20221004T113200Z
UID:9ca13e760c8f50b36129ecc5af1fd97d
DTSTART;VALUE=DATE:20221004
CLASS:PUBLIC
SUMMARY;LANGUAGE=pl:zmieszane - Wrocław
TRANSP:TRANSPARENT
CATEGORIES:KiedyWywoz, Odbior odpadow
DESCRIPTION:KiedyWywoz przypomina: odbior frakcji zmieszane w lokalizacji Wrocław
END:VEVENT
BEGIN:VEVENT
DTSTAMP:20221004T113200Z
UID:b8b8352477c429137cd8fc1a85d8a7fe
DTSTART;VALUE=DATE:20221004
CLASS:PUBLIC
SUMMARY;LANGUAGE=pl:BIO - Wrocław
TRANSP:TRANSPARENT
CATEGORIES:KiedyWywoz, Odbior odpadow
DESCRIPTION:KiedyWywoz przypomina: odbior frakcji BIO w lokalizacji Wrocław
END:VEVENT
BEGIN:VEVENT
DTSTAMP:20221004T113200Z
UID:8aa8f38edcdf86eb6f9dd21868643d84
DTSTART;VALUE=DATE:20221004
CLASS:PUBLIC
SUMMARY;LANGUAGE=pl:tworzywa - Wrocław
TRANSP:TRANSPARENT
CATEGORIES:KiedyWywoz, Odbior odpadow
DESCRIPTION:KiedyWywoz przypomina: odbior frakcji tworzywa w lokalizacji Wrocław
END:VEVENT
END:VCALENDAR";

            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig
                    {
                        BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        }
                    }
                }, new MockNetworkProvider(), new DebugLogger("debug"),
                dateProvider,
                new MockDisplayController());

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var result = sut.ParseContent(stream);

            Assert.Equal(3, result.Days[0].Bins.Length);
            Assert.Equal("Black", result.Days[0].Bins[0]);
            Assert.Equal("Brown", result.Days[0].Bins[1]);
            Assert.Equal("Yellow", result.Days[0].Bins[2]);
        }

        [TestMethod]
        public void ParseContent_should_return_Days_with_no_bins_when_no_VEVENT_for_the_day_from_today()
        {
            var dateProvider = new MockDateTimeProvider { UtcNow = new DateTime(2022, 1, 1) };

            string input = @"BEGIN:VCALENDAR
VERSION:2.0
PRODID:kwywoz-rules
CALSCALE:GREGORIAN
X-WR-CALNAME:KiedyWywóz
X-APPLE-LANGUAGE:pl
X-APPLE-REGION:PL
BEGIN:VEVENT
DTSTAMP:20221004T113200Z
UID:9ca13e760c8f50b36129ecc5af1fd97d
DTSTART;VALUE=DATE:20220104
CLASS:PUBLIC
SUMMARY;LANGUAGE=pl:zmieszane, BIO, papier - Wrocław
TRANSP:TRANSPARENT
CATEGORIES:KiedyWywoz, Odbior odpadow
DESCRIPTION:KiedyWywoz przypomina: odbior frakcji zmieszane w lokalizacji Wrocław
END:VEVENT
END:VCALENDAR";

            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig
                    {
                        BinsNames = new()
                        {
                            { "zmieszane", "Black" },
                            { "BIO", "Brown" },
                            { "tworzywa", "Yellow" },
                            { "szkło", "Green" },
                            { "papier", "Blue" },
                            { "SZOP", "Red" }
                        }
                    }
                }, new MockNetworkProvider(), new DebugLogger("debug"),
                dateProvider,
                new MockDisplayController());

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            var result = sut.ParseContent(stream);

            Assert.Equal(4, result.Days.Length);
            Assert.Equal(0, result.Days[0].Bins.Length);
            Assert.Equal(0, result.Days[1].Bins.Length);
            Assert.Equal(0, result.Days[2].Bins.Length);
            Assert.Equal(3, result.Days[3].Bins.Length);
        }

        [TestMethod]
        public void ShowSchedule_given_time_offset_before_noon_should_display_todays_bins()
        {
            var display = new MockDisplayController();
        
            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig {  }
                }, 
                new MockNetworkProvider(), 
                new DebugLogger("debug"),
                new MockDateTimeProvider {UtcNow = DateTime.Today.AddHours(9)}, //utc: 9am
                display);
        
            sut.SetValidTime();

            sut.CurrentBinsSchedule = new BinsSchedule
            {
                Days = new[]
                {
                    new BinsDay { Day = DateTime.Today, Bins = new [] { "Black" } },
                    new BinsDay { Day = DateTime.Today.AddDays(1), Bins = new [] { "Yellow" } },
                }
            };

            sut.ShowSchedule();
        
            Assert.True(BinColors.Black.Equals(display.TodaysBins[0]));
            Assert.Null(display.TomorrowsBins);
        }

        [TestMethod]
        public void ShowSchedule_given_time_offset_at_noon_should_display_todays_bins()
        {
            var display = new MockDisplayController();
        
            var sut = new BinsScheduleService(new MockConfigurationService
                {
                    ConfigSection = new WasteBinsCalendarConfig {  }
                },
                new MockNetworkProvider(),
                new DebugLogger("debug"),
                new MockDateTimeProvider { UtcNow = DateTime.Today.AddHours(12) }, //utc: 11am
                display);
        
            sut.SetValidTime();

            sut.CurrentBinsSchedule = new BinsSchedule
            {
                Days = new[]
                {
                    new BinsDay { Day = DateTime.Today, Bins = new [] { "Black" } },
                    new BinsDay { Day = DateTime.Today.AddDays(1), Bins = new [] { "Yellow" } },
                }
            };

            sut.ShowSchedule();
        
            Assert.True(BinColors.Yellow.Equals(display.TomorrowsBins[0]));
            Assert.Null(display.TodaysBins);
        }
    }
}
