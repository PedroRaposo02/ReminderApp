using System;
using System.Collections.Generic;
using System.Text;

namespace ReminderApp.Scheduling
{
    public class Reminder
    {
        public Guid Id { get; } = Guid.NewGuid();
        public string Description { get; set; } = string.Empty;
        public TimeSpan Interval { get; set; }
        public DateTime NextFireTime { get; set; }
        public bool Enabled { get; set; } = true;

        public double MinutesUntilNext => Math.Max(0, (NextFireTime - DateTime.Now).TotalMinutes);
        public double SecondsUntilNext => Math.Max(0, (NextFireTime - DateTime.Now).TotalSeconds);


        public void ScheduleNext()
        {
            NextFireTime = DateTime.Now.Add(Interval);
        }
    }
}
