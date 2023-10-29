using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    [Area("OrganizationAdmin")]
    [Authorize(Roles = "OrganizationAdmin,SuperAdmin")]
    public class OrganizationAdminBaseController : BaseController
    {
       
    }
}
