using System;
using System.Collections.Generic;
using System.Text;

namespace MyCommerce.Business.Notifications
{
    public interface INotificator
    {
        bool HaveNotification();
        List<Notification> GetNotifications();

        void Handle(Notification notification);
    }
}
