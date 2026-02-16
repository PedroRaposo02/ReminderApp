using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;
using ReminderApp.Tray;
using ReminderApp.Scheduling;
using Button = System.Windows.Controls.Button;
using MessageBox = System.Windows.MessageBox;

namespace ReminderApp.UI
{
    /// <summary>
    /// Interaction logic for ManageRemindersWindow.xaml
    /// </summary>
    public partial class ManageRemindersWindow : Window, INotifyPropertyChanged
    {
        private readonly TrayApp _trayApp;
        private DispatcherTimer _countdownTimer = null!;
        public ObservableCollection<ReminderViewModel> Reminders { get; } = [];

        private bool _isAddingNew;
        public bool IsAddingNew
        {
            get => _isAddingNew;
            set {
                if (_isAddingNew != value)
                {
                    _isAddingNew = value;
                    OnPropertyChanged();
                }
            }
        }

        private string _newDescription = "";
        public string NewDescription
        {
            get => _newDescription;
            set
            {
                _newDescription = value;
                OnPropertyChanged();   
            }
        }

        private TimeInput _newInterval = TimeInput.Zero;
        public TimeInput NewInterval
        {
            get => _newInterval;
            set
            {
                _newInterval = value;
                OnPropertyChanged();
            }
        }

        // Add property to track if any items are selected
        public bool HasSelectedItems => Reminders.Any(r => r.IsSelected);
        public ManageRemindersWindow(TrayApp trayApp)
        {
            _trayApp = trayApp;
            InitializeComponent();
            DataContext = this;
            LoadReminders();
            StartCountdownTimer();
        }

        private void LoadReminders()
        {
            var reminders = _trayApp.GetAllReminders();
            foreach (var r in reminders)
            {
                Reminders.Add(new ReminderViewModel(r));
            }

            foreach (var vm in Reminders)
            {
                vm.SelectionChanged += (s, e) => OnPropertyChanged(nameof(HasSelectedItems));
            }
        }

        private void StartCountdownTimer()         {
            _countdownTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _countdownTimer.Tick += (s, e) => UpdateAllCountdowns();
            _countdownTimer.Start();
        }

        private void UpdateAllCountdowns()
        {
            foreach (var vm in Reminders)
            {
                vm.UpdateTimeRemaining();
            }

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                vm.IsEditing = true;
                vm.EditDescription = vm.Reminder.Description;
                vm.EditInterval = TimeInput.FromTimeSpan(vm.Reminder.Interval);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                vm.Reminder.Description = vm.EditDescription;
                vm.Reminder.Interval = vm.EditInterval.TotalTimeSpan;

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

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                var result = MessageBox.Show(
                        $"Are you sure you want to delete the reminder:\n\"{vm.Reminder.Description}\"?",
                        "Confirm Delete",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    _trayApp.RemoveReminder(vm.Reminder);
                    Reminders.Remove(vm);
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void DeleteSelectedButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedReminders = Reminders.Where(r => r.IsSelected).ToList();

            if (selectedReminders.Count == 0) return;

            var result = MessageBox.Show(
                    $"Are you sure you want to delete {selectedReminders.Count} reminder(s)?",
                    "Confirm Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                foreach (var vm in selectedReminders)
                {
                    _trayApp.RemoveReminder(vm.Reminder);
                    Reminders.Remove(vm);
                }

                OnPropertyChanged(nameof(HasSelectedItems));
            }
        }

        private void AddReminderButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAddRow();
        }

        private void ShowAddRowButton_Click(object sender, RoutedEventArgs e)
        {
            ShowAddRow();
        }

        private void ShowAddRow()
        {
            IsAddingNew = true;
            NewDescription = "";
            NewDescriptionBox.Focus();
        }

        private void SaveNewButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(NewDescription) || NewInterval.TotalSeconds <= 0)
            {
                MessageBox.Show(
                    "Please enter a description and valid interval.",
                    "Invalid Input",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }

            var newReminder = new Reminder
            {
                Description = NewDescription,
                Interval = NewInterval.TotalTimeSpan,
                NextFireTime = DateTime.Now.Add(NewInterval.TotalTimeSpan)
            };

            _trayApp.AddReminder(newReminder);

            var vm = new ReminderViewModel(newReminder);
            vm.SelectionChanged += (s, ev) => OnPropertyChanged(nameof(HasSelectedItems));

            Reminders.Add(vm);

            NewInterval = TimeInput.Zero;
            IsAddingNew = false;
        }

        // Cancel adding
        private void CancelNewButton_Click(object sender, RoutedEventArgs e)
        {
            IsAddingNew = false;
        }

        private void SelectAll_Checked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Reminders)
            {
                item.IsSelected = true;
            }
        }

        private void SelectAll_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (var item in Reminders)
            {
                item.IsSelected = false;
            }
        }

        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.DataContext is ReminderViewModel vm)
            {
                // Find the next scheduled slot
                while(vm.Reminder.NextFireTime <= DateTime.Now)
                {
                    vm.Reminder.NextFireTime = vm.Reminder.NextFireTime.Add(vm.Reminder.Interval);
                }

                _trayApp.UpdateReminder(vm.Reminder);
            }
        }
    }
}
