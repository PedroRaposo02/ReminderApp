using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ReminderApp.Scheduling;

namespace ReminderApp.UI
{
    public class ReminderViewModel : INotifyPropertyChanged
    {
        public Reminder Reminder { get; }

        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
            }
        }

        private bool _isSelected;
        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged();
                SelectionChanged?.Invoke(this, EventArgs.Empty); // Notify Parent
            }
        }

        public event EventHandler SelectionChanged;

        private TimeSpan _timeRemaining;
        public TimeSpan TimeRemaining
        {
            get => _timeRemaining;
            set
            {
                _timeRemaining = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimeRemainingFormatted));
            }
        }

        public string TimeRemainingFormatted => $"{(int)TimeRemaining.TotalMinutes:D2}:{TimeRemaining.Seconds:D2}";

        // Editable Buffers
        public string EditDescription { get; set; }
        public TimeSpan EditInterval { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ReminderViewModel(Reminder reminder)
        {
            Reminder = reminder;
            EditDescription = reminder.Description;
            EditInterval = reminder.Interval;
            UpdateTimeRemaining();
        }

        public void UpdateTimeRemaining()
        {
            TimeRemaining = Reminder.NextFireTime - DateTime.Now;
            if (TimeRemaining < TimeSpan.Zero)
            {
                TimeRemaining = TimeSpan.Zero;
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
