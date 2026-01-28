using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderApp.Scheduling
{
    public class Reminder
    {
        public Guid id { get; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public TimeSpan Interval { get; set; }
        public DateTime NextFireTime { get; set; }
        public bool Enabled { get; set; } = true;

        public void ScheduleNext()
        {
            NextFireTime = DateTime.Now.Add(Interval);
        }
    }
}
