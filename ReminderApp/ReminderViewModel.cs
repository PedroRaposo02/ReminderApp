using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using ReminderApp.Scheduling;

namespace ReminderApp.UI
{
    public class ReminderViewModel(Reminder reminder) : INotifyPropertyChanged
    {
        public Reminder Reminder { get; } = reminder;

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
            }
        }

        // Editable Buffers
        public string EditDescription { get; set; } = reminder.Description;
        public double EditIntervalMinutes { get; set; } = reminder.Interval.TotalMinutes;
        public string FormattedInterval => TimeSpan.FromMinutes(EditIntervalMinutes).ToString(@"mm\:ss");

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null!)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
