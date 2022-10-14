using MakoIoT.Device.Services.Interface;
using MakoIoT.Samples.WBC.Device.Services;
using Microsoft.Extensions.Logging;

namespace MakoIoT.Samples.WBC.Device.Tasks
{
    public class ShowBinsScheduleTask : ITask
    {
        private readonly IBinsScheduleService _binsScheduleService;

        public ShowBinsScheduleTask(IBinsScheduleService binsScheduleService)
        {
            Id = nameof(ShowBinsScheduleTask);
            _binsScheduleService = binsScheduleService;
        }

        public string Id { get; }
        public void Execute()
        {
            _binsScheduleService.ShowSchedule();
        }
    }
}
