using MakoIoT.Device.Displays.Led;
using MakoIoT.Samples.WBC.Device.Services;

namespace MakoIoT.Samples.WBC.Device.Tests.Mocks
{
    public class MockDisplayController : IDisplayController
    {
        public Color[] TodaysBins { get; private set; }
        public Color[] TomorrowsBins { get; private set; }

        public void DisplayTodaysBins(Color[] bins)
        {
            TodaysBins = bins;
        }

        public void DisplayTomorrowsBins(Color[] bins)
        {
            TomorrowsBins = bins;
        }

        public void Blank()
        {
        }

        public void DisplayError()
        {
            
        }

        public void DisplayUpdating()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayUpdatingError()
        {
            throw new System.NotImplementedException();
        }

        public void DisplayConfigMode()
        {
            throw new System.NotImplementedException();
        }
    }
}
