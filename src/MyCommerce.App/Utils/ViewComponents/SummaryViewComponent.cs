using Microsoft.AspNetCore.Mvc;
using MyCommerce.Business.Notifications;
using System.Threading.Tasks;

namespace MyCommerce.App.Extensions
{
    public class SummaryViewComponent : ViewComponent
    {
        private readonly INotificator _notificator;
        public SummaryViewComponent(INotificator notificator)
        {
            _notificator = notificator;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = await Task.FromResult(_notificator.GetNotifications());
            notifications.ForEach(n => ViewData.ModelState.AddModelError(string.Empty, n.Message));
            return View();
        }
    }
}
