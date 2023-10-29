using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Views.Shared
{
    public class ToastNotViewCompanent : ViewComponent
    {
        private readonly INotyfService _notifyService;

        public ToastNotViewCompanent(INotyfService notifyService)
        {
            _notifyService = notifyService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var notifications = _notifyService.GetNotifications();
            return View(notifications);
        }
    }
}
