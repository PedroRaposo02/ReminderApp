using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
        private DateTime _nextFireTime;
        public DateTime NextFireTime
        {
            get => _nextFireTime;
            set
            {
                if (_nextFireTime != value)
                {
                    _nextFireTime = value;
                    OnPropertyChanged();
                }
            }
        }
        public bool Enabled { get; set; } = true;
        
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ScheduleNext()
        {
            NextFireTime = DateTime.Now.Add(Interval);
        }
    }
}
