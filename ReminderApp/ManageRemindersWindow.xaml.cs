using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using ReminderApp.Scheduling;
using ReminderApp.Tray;
using Button = System.Windows.Controls.Button;

namespace ReminderApp.UI
{
    /// <summary>
    /// Interaction logic for ManageRemindersWindow.xaml
    /// </summary>
    public partial class ManageRemindersWindow : Window
    {
        private TrayApp _trayApp;
        public ObservableCollection<ReminderViewModel> Reminders { get; } = new();
        public ManageRemindersWindow(TrayApp trayApp)
        {
            _trayApp = trayApp;
            InitializeComponent();
            LoadReminders();
            DataContext = this;
        }

        private void LoadReminders()
        {
            var reminders = _trayApp.GetReminders();
            foreach (var r in reminders)
            {
                Reminders.Add(new ReminderViewModel(r));
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                vm.IsEditing = true;
                vm.EditDescription = vm.Reminder.Description;
                vm.EditIntervalMinutes = vm.Reminder.Interval.TotalMinutes;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                vm.Reminder.Description = vm.EditDescription;
                vm.Reminder.Interval = TimeSpan.FromMinutes(vm.EditIntervalMinutes);

                _trayApp.UpdateReminder(vm.Reminder);
                vm.IsEditing = false;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                vm.IsEditing = false;
            }
        }
    }
}
