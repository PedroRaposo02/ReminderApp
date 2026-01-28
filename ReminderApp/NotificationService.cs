using Microsoft.Toolkit.Uwp.Notifications;

namespace ReminderApp.Notifications
{
    public static class NotificationService
    {
        public static void ShowToast(string title, string message)
        {
            new ToastContentBuilder()
                .AddText(title)
                .AddText(message)
                .Show();
        }
    }
}
