using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ReminderApp.Scheduling
{
    public class Reminder : INotifyPropertyChanged
    {
        public Guid Id { get; } = Guid.NewGuid();

        private string _description = string.Empty;
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }

        private TimeSpan _interval;
        public TimeSpan Interval
        {
            get => _interval;
            set
            {
                if (_interval != value)
                {
                    _interval = value;
                    OnPropertyChanged(nameof(Interval));
                }
            }
        }
        public DateTime NextFireTime { get; set; }
        public bool Enabled { get; set; } = true;

        public double MinutesUntilNext => Math.Max(0, (NextFireTime - DateTime.Now).TotalMinutes);
        public double SecondsUntilNext => Math.Max(0, (NextFireTime - DateTime.Now).TotalSeconds);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ScheduleNext()
        {
            NextFireTime = DateTime.Now.Add(Interval);
        }
    }
}
