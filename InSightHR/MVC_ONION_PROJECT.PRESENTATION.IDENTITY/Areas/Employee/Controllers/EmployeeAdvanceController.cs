using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffServices;
using MVC_ONION_PROJECT.APPLICATION.Services.TimeOffTypeService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class EmployeeAdvanceController : EmployeeBaseController
    {
        private readonly IAdvancePaymentService _advancePaymentService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailHelper _emailHelper;

        public EmployeeAdvanceController(IAdvancePaymentService advancePaymentService, IMapper mapper, IHttpContextAccessor contextAccessor, IEmployeeService employeeService, IEmailService emailService, SignInManager<IdentityUser> signInManager, IEmailHelper emailHelper)
        {
            _advancePaymentService = advancePaymentService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
            _emailService = emailService;
            _signInManager = signInManager;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _advancePaymentService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeAdvanceListVm>>(result.Data));
            }
            var userAdvanceList = result.Data.FindAll(x => x.CreatedBy == _signInManager.Context.User?.FindFirstValue(ClaimTypes.NameIdentifier));
            var userAdvanceListVM = _mapper.Map<List<EmployeeAdvanceListVm>>(userAdvanceList);
            return View(userAdvanceListVM);
        }

        [HttpGet]
        public IActionResult AddAdvancePayment()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddAdvancePayment(EmployeeAdvanceCreateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(userId));
            model.EmployeeId = employee.Data.Id;
            model.ReturnStatus = DOMAIN.ENUMS.ReturnStatus.Pending;
            var addResult = await _advancePaymentService.AddAsync(_mapper.Map<AdvancePaymentCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                return View(model);
            }
            var mail = await _emailHelper.AvansMailIstek(employee.Data.Name, employee.Data.Surname, model.AdvancePrice);
            
            await _emailService.SendMail("Avans İsteği Hk.", mail ,"muhammed.dereli@bilgeadamboost.com");
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
          
            var result = await _advancePaymentService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditAdvancePayment(Guid id)
        {
            var result = await _advancePaymentService.GetByIdAsync(id);


            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeAdvanceUpdateVm>(result.Data));
            }
            if (result.Data.ReturnStatus != DOMAIN.ENUMS.ReturnStatus.Pending)
            {
                ErrorNoty("Cevaplanmış talep değiştirilemez.");
                return RedirectToAction(nameof(Index));
            }
            var categoryEditVm = _mapper.Map<EmployeeAdvanceUpdateVm>(result.Data);
            return View(categoryEditVm);
        }
        [HttpPost]

        public async Task<IActionResult> EditAdvancePayment(EmployeeAdvanceUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var categoryeditdto = _mapper.Map<AdvancePaymentUpdateDto>(model);
            categoryeditdto.ReturnStatus = DOMAIN.ENUMS.ReturnStatus.Pending;
            var result = await _advancePaymentService.UpdateAsync(categoryeditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            SuccessNoty(result.Message);
            var resultVM = _mapper.Map<EmployeeAdvanceUpdateVm>(result.Data);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DetailsAdvancePayment(Guid id)
        {
            var result = await _advancePaymentService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeAdvanceDetailVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var paymentDetailVm = _mapper.Map<EmployeeAdvanceDetailVm>(result.Data);
            return View(paymentDetailVm);
        }
    }
}
