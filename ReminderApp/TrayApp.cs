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
        private NotifyIcon _trayIcon = null!;
        private readonly ReminderScheduler scheduler;
        private ManageRemindersWindow? _manageWindow;

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
            _trayIcon = new NotifyIcon();

            var iconUri = new Uri("pack://application:,,,/Icons/tray.ico");
            var iconStream = Application.GetResourceStream(iconUri);

            _trayIcon.Icon = new Icon(iconStream.Stream);

            _trayIcon.Text = "Reminder App";
            _trayIcon.Visible = true;

            _trayIcon.MouseClick += TrayIcon_MouseClick;

            // Right-click context menu
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Open", null, (s, e) => OpenManageWindow());
            contextMenu.Items.Add("Exit", null, (s, e) => ExitApplication());
            _trayIcon.ContextMenuStrip = contextMenu;

        }

        private void OpenManageWindow()
        {
            if (_manageWindow == null || !_manageWindow.IsVisible)
            {
                _manageWindow = new ManageRemindersWindow(this); // Pass 'this' (TrayApp)
                _manageWindow.Closing += Window_Closing;
                _manageWindow.Show();
            }
            else if (!_manageWindow.IsVisible)
            {
                _manageWindow.Show();
            }

            _manageWindow.Activate();
            _manageWindow.WindowState = WindowState.Normal;
        }

        private void TrayIcon_MouseClick(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) OpenManageWindow();
        }

        private void Window_Closing(object? sender, CancelEventArgs e)
        {
            // Cancel the close and hide instead
            e.Cancel = true;
            _manageWindow?.Hide();
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
            scheduler.RemoveReminder(reminder);
            scheduler.AddReminder(reminder);
            _persistedReminders.RemoveAll(r => r.Id == reminder.Id);
            _persistedReminders.Add(reminder);
            ReminderStore.Save(_persistedReminders);
        }

        // Returns only persisted reminders
        public List<Reminder> GetReminders()
        {
            return [.. _persistedReminders]; // Return a copy
        }

        // Returns all reminders including non-persisted ones
        public List<Reminder> GetAllReminders()
        {
            return [.. scheduler.GetReminders()];
        }

        private void ExitApplication()
        {
            Dispose();
            Application.Current.Shutdown();
        }
      
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }
        }
    }
}
