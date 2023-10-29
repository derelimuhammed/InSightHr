using Microsoft.AspNetCore.Mvc;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class EmployeeSettingsController : EmployeeBaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
