using System;
using System.Windows;
using ReminderApp.Tray;

namespace ReminderApp.UI
{
    public partial class TrayMenuWindow : Window
    {
        private readonly Action _openAction;
        private readonly Action _exitAction;

        public TrayMenuWindow(Action openAction, Action exitAction)
        {
            InitializeComponent();
            _openAction = openAction;
            _exitAction = exitAction;
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            // Close the menu if user clicks elsewhere
            Hide();
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            _openAction?.Invoke();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            _exitAction?.Invoke();
        }
    }
}
