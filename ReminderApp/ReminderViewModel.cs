using System;
using System.Collections.Generic;
using System.Text;
using ReminderApp.Scheduling;

namespace ReminderApp.UI
{
    public class ReminderViewModel
    {
        public Reminder Reminder { get; }

        public bool IsEditing { get; set; } = false;
        public bool IsSelected { get; set; } = false;

        // Editable Buffers
        public string EditDescription { get; set; }
        public double EditIntervalMinutes { get; set; }
    
        public ReminderViewModel(Reminder reminder)
        {
            Reminder = reminder;
            EditDescription = reminder.Description;
            EditIntervalMinutes = reminder.Interval.TotalMinutes;
        }
    }
}
