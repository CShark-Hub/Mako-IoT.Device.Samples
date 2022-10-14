using MakoIoT.Device.Displays.Led;

namespace MakoIoT.Samples.WBC.Device.Services
{
    public interface IDisplayController
    {
        void DisplayTodaysBins(Color[] bins);
        void DisplayTomorrowsBins(Color[] bins);
        void Blank();
        void DisplayError();
        void DisplayUpdating();
        void DisplayUpdatingError();
        void DisplayConfigMode();
    }
}
