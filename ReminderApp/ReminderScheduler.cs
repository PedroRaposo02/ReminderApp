namespace ReminderApp.Scheduling
{
    public class ReminderScheduler : IDisposable
    {
        private readonly List<Reminder> _reminders = new();
        private readonly PeriodicTimer _timer;
        private readonly CancellationTokenSource _cts = new();

        public event Action<Reminder>? ReminderFired;

        public ReminderScheduler()
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(2));
            _ = RunAsync();
        }
        public IReadOnlyList<Reminder> Reminders => _reminders.AsReadOnly();

        public void AddReminder(Reminder reminder)
        {
            reminder.ScheduleNext();
            _reminders.Add(reminder);
        }

        public void RemoveReminder(Guid id)
        {
            _reminders.RemoveAll(r => r.id == id);
        }

        public async Task RunAsync()
        {
            try
            {
                while (await _timer.WaitForNextTickAsync(_cts.Token))
                {
                    Tick();
                }
            }
            catch (OperationCanceledException)
            {
                // Graceful shutdown
            }
        }


        private void Tick()
        {
            var now = DateTime.Now;
            
            foreach(var reminder in _reminders.Where(r => r.Enabled && now >= r.NextFireTime))
            {
                ReminderFired?.Invoke(reminder);
                reminder.ScheduleNext();
            }
        }

        public void Dispose()
        {
            _cts.Cancel();
            _timer.Dispose();
            _cts.Dispose();
        }
    }
}
