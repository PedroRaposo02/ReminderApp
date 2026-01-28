using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ReminderApp.Scheduling;

namespace ReminderApp
{
    public partial class AddReminderWindow : Window
    {
        public Reminder? CreatedReminder { get; private set; }
        public AddReminderWindow()
        {
            InitializeComponent();
        }

        private void On_Click(object sender, RoutedEventArgs e)
        {
            CreatedReminder = new Reminder
            {
                Description = descriptionTextBox.Text,
                Interval = TimeSpan.FromMinutes(double.Parse(intervalTextBox.Text))
            };
        }
    }
}
