using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class BaseController : Controller
    {
        public INotyfService _notfyService => HttpContext.RequestServices.GetService(typeof(INotyfService)) as INotyfService;
        

        protected void SuccessNoty(string message)
        {
            _notfyService.Success(message);
        }
        protected void ErrorNoty(string message)
        {
            _notfyService.Error(message);
        }
        protected void InformationModal(string message)
        {
            _notfyService.Information(message,50);
        }
    }
}
