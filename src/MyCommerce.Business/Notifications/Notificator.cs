using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommerce.Business.Notifications
{
    public class Notificator : INotificator
    {
        private List<Notification> _notifications;

        public Notificator()
        {
            _notifications = new List<Notification>();
        }
        public List<Notification> GetNotifications()
        {
            return _notifications;
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }

        public bool HaveNotification()
        {
            return _notifications.Any();
        }
    }
}
