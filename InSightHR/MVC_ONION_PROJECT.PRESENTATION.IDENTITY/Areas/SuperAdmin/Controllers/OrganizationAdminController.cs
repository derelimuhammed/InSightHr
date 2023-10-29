using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.APPLICATION.Services.PackageService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Models.OrganizationAdminVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.SuperAdmin.Controllers
{
    public class OrganizationAdminController : SuperAdminBaseController
    {

        private readonly IOrganizationService _organizationService;


        public OrganizationAdminController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        [HttpGet("SuperAdmin/OrganizationAdmin/index/{id}")]
        public async Task<IActionResult> Index(string id)
        {
            var result = await _organizationService.GetByIdAsync(Guid.Parse(id));
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View();
            }
            HttpContext.Session.SetString("OrganizationId", result.Data.Id.ToString());
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View();
            }
            return RedirectToAction("Index", "Employee", new { area = "OrganizationAdmin" });
        }
       
    
}
    }

