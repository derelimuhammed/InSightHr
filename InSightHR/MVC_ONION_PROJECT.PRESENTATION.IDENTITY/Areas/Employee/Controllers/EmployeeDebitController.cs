using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrgAssetService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class EmployeeDebitController : EmployeeBaseController
    {

        private readonly IOrgAssetService _orgAssetService;
        private readonly IEmployeeDebitService _employeeDebitService;
		private readonly IEmployeeService _employeeService;
		private readonly IHttpContextAccessor _contextAccessor;
		private readonly IMapper _mapper;

		public EmployeeDebitController(IOrgAssetService orgAssetService, IEmployeeDebitService employeeDebitService, IMapper mapper, IHttpContextAccessor contextAccessor, IEmployeeService employeeService)
		{
			_orgAssetService = orgAssetService;
			_employeeDebitService = employeeDebitService;
			_mapper = mapper;
			_contextAccessor = contextAccessor;
			_employeeService = employeeService;
		}

		public async Task<IActionResult> Index()
        {
			var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
			var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(userId));
			var result = await _employeeDebitService.GetByEmployeeId(employee.Data.Id); 
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return View(_mapper.Map<List<EmployeeDebitListVm>>(result.Data));
			}
			var employeeDebitListVm = _mapper.Map<List<EmployeeDebitListVm>>(result.Data);
			if (employeeDebitListVm.Count <= 0)
			{
				return View();
			}
			else
			{			
				foreach (var item in employeeDebitListVm)
				{
					var orgAsset = await _orgAssetService.GetByIdAsync(item.OrgAssetId);
					item.DebitName = orgAsset.Data.Name;
				}

				return View(employeeDebitListVm);
			}
		}


		public async Task<ActionResult> AcceptDebit(Guid id)
		{
			var result = await _employeeDebitService.AcceptDebitAsync(id);
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return RedirectToAction(nameof(Index));
			}
			SuccessNoty(result.Message);
			return RedirectToAction(nameof(Index));
		}
        

        public async Task<ActionResult> RejectDebit(Guid id)
		{
			var result = await _employeeDebitService.RejectDebitAsync(id);
			if (!result.IsSuccess)
			{
				ErrorNoty(result.Message);
				return RedirectToAction(nameof(Index));
			}
			SuccessNoty(result.Message);
			return RedirectToAction(nameof(Index));
		}
        public async Task<ActionResult> DetailsEmployeeDebit(Guid id)
        {
            var result = await _employeeDebitService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return View(_mapper.Map<EmployeeDebitDetailsVm>(result.Data));
        }
    }
}
