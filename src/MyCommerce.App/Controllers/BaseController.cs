using Microsoft.AspNetCore.Mvc;
using MyCommerce.Business.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCommerce.App.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly INotificator _notificator;
        protected BaseController(INotificator notificator)
        {
            _notificator = notificator;
        }

        protected bool ValidOperation()
        {
            return !_notificator.HaveNotification();
        }
    }
}
