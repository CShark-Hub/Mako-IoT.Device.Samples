﻿using System;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Utilities.TimeZones;
using MakoIoT.Samples.WBC.Device.Configuration;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        private readonly TimeZone _tz;

        public DateTime UtcNow => DateTime.UtcNow;
        public DateTime Now => _tz == null ? DateTime.UtcNow : _tz.GetLocalTime(DateTime.UtcNow);

        public DateTimeProvider(IConfigurationService configService, ILog logger)
        {
            var config = (WasteBinsCalendarConfig)configService.GetConfigSection(WasteBinsCalendarConfig.SectionName,
                typeof(WasteBinsCalendarConfig));

            try
            {
                _tz = TimeZoneConverter.FromPosixString(config.Timezone);
            }
            catch (Exception e)
            {
                logger.Error("Invalid time zone configuration. UTC time will be used.", e);
            }
        }
    }
}
