using System;
using System.Collections;
using System.IO;
using System.Net.Http;
using MakoIoT.Device.SecureClient.Services;
using MakoIoT.Device.Services.Interface;
using MakoIoT.Device.Utilities.ICalParser;
using MakoIoT.Device.Utilities.Invoker;
using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Extensions;
using MakoIoT.Samples.WBC.Device.Model;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public class BinsScheduleService : IBinsScheduleService
    {
        private readonly WasteBinsCalendarConfig _config;
        private readonly INetworkProvider _networkProvider;
        private readonly ILog _logger;
        private readonly HttpClient _httpClient;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IDisplayController _displayController;
        private bool _hasValidTime;


        public BinsSchedule CurrentBinsSchedule { get; set; }

        public BinsScheduleService(IConfigurationService configService, 
            INetworkProvider networkProvider, 
            ILog logger, 
            IDateTimeProvider timeProvider, 
            IDisplayController displayController,
            IClientProvider clientProvider)
        {
            _config = (WasteBinsCalendarConfig)configService
                .GetConfigSection(WasteBinsCalendarConfig.SectionName, typeof(WasteBinsCalendarConfig));
            _networkProvider = networkProvider;
            _logger = logger;
            _timeProvider = timeProvider;
            _displayController = displayController;

            if (string.IsNullOrEmpty(_config.CalendarUrl))
            {
                _displayController.DisplayUpdatingError();
                return;
            }

            _httpClient = clientProvider.GetSecureHttpClient(_config.CalendarUrl);
        }

        public void UpdateSchedule()
        {
            EnsureConnection();

            Invoker.Retry(() =>
            {
                _logger.Trace($"Sending GET request");
                using var response = _httpClient.Get(_config.CalendarUrl);
                response.EnsureSuccessStatusCode();
                CurrentBinsSchedule = ParseContent(response.Content.ReadAsStream());
            }, 3, (ex, attempt) =>
            {
                _logger.Error("HttpClient.Get exception", ex);
                return true;
            });
            
            _logger.Information($"Schedule updated at {_timeProvider.UtcNow} (UTC)");
           
        }

        public void ShowSchedule()
        {
            if (!_hasValidTime)
            {
                EnsureConnection();
            }

            if (_hasValidTime)
            {
                var localTime = _timeProvider.Now;
                _logger.Trace($"Local time is {localTime}");
                if (localTime.Hour < 12)
                {
                    var bins = GetBinsForDate(localTime);
                    if (bins != null)
                    {
                        _displayController.DisplayTodaysBins(bins.ToColorsArray());
                        return;
                    }
                    _logger.Error("No data for today");
                }
                else
                {
                    var bins = GetBinsForDate(localTime.AddDays(1));
                    if (bins != null)
                    {
                        _displayController.DisplayTomorrowsBins(bins.ToColorsArray());
                        return;
                    }
                    _logger.Error("No data for tomorrow");
                }
            }
            else
            {
                _logger.Error("Current date/time not available");
            }

            _displayController.DisplayError();
        }

        public BinsSchedule ParseContent(Stream contentStream)
        {
            var parser = new Parser();
            var days = new ArrayList();
            using (var reader = new StreamReader(contentStream))
            {
                parser.Parse(reader, e => AddBinsForDay(days, e.DtStart, GetBinsFromSummary(e.Summary)));
            }

            AddEmptyDays(days, _timeProvider.Now.Date);

            return new BinsSchedule { Days = (BinsDay[])days.ToArray(typeof(BinsDay)) };
        }

        public string[] GetBinsFromSummary(string s)
        {
            var bins = new ArrayList();
            foreach (string name in _config.BinsNames.Keys)
            {
                if (s.ToLower().Contains(name.ToLower()))
                {
                    bins.Add(_config.BinsNames[name]);
                }
            }

            return (string[])bins.ToArray(typeof(string));
        }

        public void AddEmptyDays(ArrayList days, DateTime today)
        {
            DateTime lastDay = today;
            for (int i = 0; i < days.Count; i++)
            {
                var day = (BinsDay)days[i];
                while (lastDay.Date < day.Day.Date)
                {
                    days.Insert(i, new BinsDay { Day = lastDay, Bins = new string[0]});
                    lastDay = lastDay.AddDays(1);
                }
            }
        }

        public BinsDay GetBinsForDate(DateTime date)
        {
            BinsDay bins = null;

            if (CurrentBinsSchedule != null)
                bins = CurrentBinsSchedule.Days.FindByDate(date);

            if (bins == null)
            {
                _displayController.DisplayUpdating();
                try
                {
                    UpdateSchedule();
                    bins = CurrentBinsSchedule.Days.FindByDate(date);

                }
                catch (Exception ex)
                {
                    _logger.Error("Error updating schedule", ex);
                    _displayController.DisplayUpdatingError();
                }
            }
            return bins;
        }

        public void SetValidTime() //TODO: temporary, for testing
        {
            _hasValidTime = true;
        }

        private void AddBinsForDay(ArrayList days, DateTime date, string[] bins)
        {
            foreach (BinsDay day in days)
            {
                if (day.Day == date)
                {
                    var newBins = new string[day.Bins.Length + bins.Length];
                    day.Bins.CopyTo(newBins, 0);
                    bins.CopyTo(newBins, day.Bins.Length);
                    day.Bins = newBins;
                    return;
                }
            }

            days.Add(new BinsDay { Day = date, Bins = bins });
        }

        private void EnsureConnection()
        {
            if (!_networkProvider.IsConnected)
            {
                _logger.Trace("Network not connected");

                _networkProvider.Connect();

                if (!_networkProvider.IsConnected)
                    throw new Exception("Could not connect to network");

                _logger.Trace("Connected to WIFI");
            }

            _hasValidTime = true;
        }
    }
}
