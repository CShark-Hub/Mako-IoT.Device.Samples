using MakoIoT.Samples.WBC.Device.Configuration;
using MakoIoT.Samples.WBC.Device.Model;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public interface IBinsScheduleService
    {
        void UpdateSchedule();
        void ShowSchedule();
        BinsSchedule CurrentBinsSchedule { get; }
    }
}