using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ReminderApp.Notifications;
using ReminderApp.Persistence;
using ReminderApp.Scheduling;
using Application = System.Windows.Application;

namespace ReminderApp.Tray
{
    public class TrayApp : IDisposable
    {
        private NotifyIcon _trayIcon;
        private ReminderScheduler scheduler;

        public TrayApp()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Text = "Reminder App",
                Visible = true,
                ContextMenuStrip = BuildContextMenu()
            };

            scheduler = new ReminderScheduler();

            scheduler.ReminderFired += reminder =>
            {
                NotificationService.ShowToast("Reminder", reminder.Description);
            };

            // Load saved reminders
            var reminders = ReminderStore.Load();
            foreach (var r in reminders) AddReminder(r);

            // Add bootstrap/default reminder (does not get saved)
            var bootstrap = new Reminder
                {
                    Description = "Drink Water",
                    Interval = TimeSpan.FromSeconds(2)
                };
            scheduler.AddReminder(bootstrap);

            var window = new AddReminderWindow();
            if (window.ShowDialog() == true)
            {
                var newReminder = window.CreatedReminder;
                AddReminder(newReminder);
            }
        }

        private ContextMenuStrip BuildContextMenu()
        {
            var menu = new ContextMenuStrip();

            var exitItem = new ToolStripMenuItem("Exit");
            exitItem.Click += (_, _) => Application.Current.Shutdown();

            menu.Items.Add(exitItem);

            return menu;
        }

        private void AddReminder(Reminder reminder)
        {
            scheduler.AddReminder(reminder);
            ReminderStore.Save([..scheduler.Reminders]);
        }

        private void RemoveReminder(Reminder reminder)
        {
            scheduler.RemoveReminder(reminder); 
            ReminderStore.save([.. scheduler.Reminders]);
        }

        public void Dispose()
        {
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }
    }
}
