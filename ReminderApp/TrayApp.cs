using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using ReminderApp.Notifications;
using ReminderApp.Persistence;
using ReminderApp.Scheduling;
using ReminderApp.UI;
using Application = System.Windows.Application;

namespace ReminderApp.Tray
{
    public class TrayApp : IDisposable
    {
        private NotifyIcon _trayIcon;
        private readonly ReminderScheduler scheduler;
        private ManageRemindersWindow? manageWindow;

        private readonly List<Reminder> _persistedReminders = [];

        public TrayApp()
        {
            InitializeTrayIcon();

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
                    Interval = TimeSpan.FromMinutes(1)
                };
            scheduler.AddReminder(bootstrap);
        }

        private void InitializeTrayIcon()
        {
            _trayIcon = new NotifyIcon
            {
                Icon = SystemIcons.Information,
                Text = "Reminder App",
                Visible = true,
            };

            _trayIcon.Click += (s, e) => OpenManageWindow();

            // Right-click context menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open", null, (s, e) => OpenManageWindow());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
            _trayIcon.ContextMenuStrip = contextMenu;

        }

        private void OpenManageWindow()
        {
            if (manageWindow == null || !manageWindow.IsVisible)
            {
                manageWindow = new ManageRemindersWindow(this); // Pass 'this' (TrayApp)
                manageWindow.Show();
            }
            else
            {
                manageWindow.Activate();
            }
        }

        private void ExitApplication()
        {
            _trayIcon.Dispose();
            Dispose();
            Application.Current.Shutdown();
        }

        public void AddReminder(Reminder reminder)
        {
            scheduler.AddReminder(reminder);
            _persistedReminders.Add(reminder);
            ReminderStore.Save(_persistedReminders);
        }

        public void RemoveReminder(Reminder reminder)
        {
            scheduler.RemoveReminder(reminder);
            _persistedReminders.Remove(reminder);
            ReminderStore.Save(_persistedReminders);
        }

        public void UpdateReminder(Reminder reminder) 
        {
            scheduler.UpdateReminder(reminder);
            _persistedReminders.RemoveAll(r => r.Id == reminder.Id);
            _persistedReminders.Add(reminder);
            ReminderStore.Save(_persistedReminders);
        }

        public List<Reminder> GetReminders()
        {
            return [.. _persistedReminders]; // Return a copy
        }
      
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _trayIcon.Visible = false;
            _trayIcon.Dispose();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            // Cancel the close and hide instead
            e.Cancel = true;
            manageWindow?.Hide();
        }
    }
}
