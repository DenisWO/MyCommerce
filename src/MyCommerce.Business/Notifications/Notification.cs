using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyCommerce.Business.Notifications
{
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }
        public string Message { get; }
    }

}
