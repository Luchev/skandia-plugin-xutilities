using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace xUtilities
{
    static class NotificationManager
    {
        private static Thread notificationThread;
        private static NotificationWindow Notification;
        public static void Initialize()
        {
            StartNotificationThread();
        }
        public static void Terminate()
        {
            if (Notification != null && Notification.InvokeRequired)
            {
                Notification.Invoke((MethodInvoker)delegate
                {
                    Notification.Close();
                    Notification.Dispose();
                });
            }
            else
            {
                if (Notification != null && !Notification.IsDisposed)
                {
                    Notification.Close();
                    Notification.Dispose();
                }
            }
        }
        public static void SendNotification(NotificationType _type, string labelText)
        {
            Notification.SetType(_type);
            Notification.Send(labelText);
            Notification.Show();
            Task.Delay(TimeSpan.FromMilliseconds(2000)).ContinueWith((Task) =>
            {
                Notification.Hide();
            });
        }
        private static void StartNotificationThread()
        {
            if (notificationThread == null ||
            notificationThread.ThreadState == System.Threading.ThreadState.Aborted ||
            notificationThread.ThreadState == System.Threading.ThreadState.Stopped ||
            notificationThread.ThreadState == System.Threading.ThreadState.Unstarted)
            {
                notificationThread = new Thread(SettingsUIThreadStartMethod);
                notificationThread.SetApartmentState(ApartmentState.STA);
                notificationThread.Start();
                return;
            }
            else
            {
                H.Log(0, "[ERROR]NotificationManager failed to initialize");
                return;
            }
        }
        // Used by settingUIThread as starting method
        private static void SettingsUIThreadStartMethod()
        {
            if (Notification == null)
            {
                Notification = new NotificationWindow();
            }
            H.Log(0, "[NM]Initialized");
        }
    }
    public enum NotificationType
    {
        Text,
        YesNo,
        OkCancel,
        Input
    }
}
