using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ReminderApp.UI
{
    public class TimeInput : INotifyPropertyChanged
    {
        public static readonly TimeInput Zero = new() { _hours = 0, _minutes = 0, _seconds = 0 };

        private int _hours;
        private int _minutes;
        private int _seconds;

        public int Hours
        {
            get => _hours;
            set
            {
                _hours = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalTimeSpan));
            }
        }

        public int Minutes
        {
            get => _minutes;
            set
            {
                _minutes = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalTimeSpan));
            }
        }

        public int Seconds
        {
            get => _seconds;
            set
            {
                _seconds = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalTimeSpan));
            }
        }

        public TimeSpan TotalTimeSpan => TimeSpan.FromHours(Hours)
            .Add(TimeSpan.FromMinutes(Minutes))
            .Add(TimeSpan.FromSeconds(Seconds));

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Constructor from TimeSpan
        public static TimeInput FromTimeSpan(TimeSpan ts)
        {
            return new TimeInput
            {
                _hours = (int)ts.TotalHours,
                _minutes = ts.Minutes,
                _seconds = ts.Seconds
            };
        }

        public int TotalSeconds => (int)TotalTimeSpan.TotalSeconds;
    }
}
