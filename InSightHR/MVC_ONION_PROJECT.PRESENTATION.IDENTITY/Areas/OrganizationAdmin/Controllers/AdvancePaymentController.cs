using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AdvancePaymentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms;


namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class AdvancePaymentController : OrganizationAdminBaseController
    {
        private readonly IAdvancePaymentService _advancePaymentService;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEnumHelperService _enumHelper;
        private readonly IEmailHelper _emailHelper;


        public AdvancePaymentController(IAdvancePaymentService advancePaymentService, IMapper mapper, IEmployeeService employeeService, IEmailService emailService, UserManager<IdentityUser> userManager, IEnumHelperService enumHelper, IEmailHelper emailHelper)
        {
            _advancePaymentService = advancePaymentService;
            _mapper = mapper;
            _employeeService = employeeService;
            _emailService = emailService;
            _userManager = userManager;
            _enumHelper = enumHelper;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _advancePaymentService.GetAllAsync();

            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<AdvanceListVm>>(result.Data));
            }
            var filterResult = result.Data.FindAll(x => x.ReturnStatus == DOMAIN.ENUMS.ReturnStatus.Pending);
            var model = _mapper.Map<List<AdvanceListVm>>(filterResult);
            foreach (var item in model)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }
            return View(model);
        }
        [HttpGet("OrganizationAdmin/AdvancePayment/GetFilter/{filter?}")]
        public async Task<IActionResult> GetFilter(string filter)
        {
            var result = await _advancePaymentService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<AdvanceListVm>>(result.Data));
            }
            var model = _mapper.Map<List<AdvanceListVm>>(result.Data);
            foreach (var item in model)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }
            if (filter == "Hepsi")
                return PartialView("_AdvanceListPartial", model);
            var filtermodel = model.FindAll(x => _enumHelper.GetDisplayName(x.ReturnStatus) == filter).ToList();
            SuccessNoty(result.Message);
            return PartialView("_AdvanceListPartial", filtermodel);
        }
        [HttpPost("OrganizationAdmin/AdvancePayment/SetAdvancePayment/{Id}/{returnStatusstring}")]
        public async Task SetAdvancePayment(string Id, string returnStatusstring)
        {
            ReturnStatus? returnStatus=null;
            foreach (ReturnStatus status in Enum.GetValues(typeof(ReturnStatus)))
            {
                if (_enumHelper.GetDisplayName(status) == returnStatusstring)
                {
                    returnStatus = status;
                    break;
                }
            }
            if (returnStatus == null)
            {
                return;
            } 
            var advance = await _advancePaymentService.GetByIdAsync(Guid.Parse(Id));
            if (!ModelState.IsValid)
            {
                return;
            }
            var advancePaymentEditdto = _mapper.Map<AdvancePaymentUpdateDto>(advance.Data);
            advancePaymentEditdto.ReturnStatus = returnStatus??ReturnStatus.Returned;
            var result = await _advancePaymentService.UpdateAsync(advancePaymentEditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return;
            }
            if (advance.Data.ReturnStatus != returnStatus)
            {
                var employee = await _employeeService.GetByIdAsync(result.Data.EmployeeId);
                var identityuser = await _userManager.FindByIdAsync(employee.Data.IdentityId.ToString());
                var mail = await _emailHelper.AvansGuncellemeMail(result.Data.CreatedDate, result.Data.AdvancePrice, returnStatus);
                await _emailService.SendMail($"{result.Data.CreatedDate} - Tarihinizdeki avans isteğiniz", mail, identityuser.Email);
            }
            return;
        }
        [HttpGet("OrganizationAdmin/AdvancePayment/RejectedResponse/{Id}")]
        public async Task<IActionResult> RejectedResponse(string Id)
        {
            var result = await _advancePaymentService.GetByIdAsync(Guid.Parse(Id));
            if (result.Data.ReturnStatus == ReturnStatus.Rejected)
            {
                ErrorNoty("Reddedilmiş bir izni tekrardan rededemezsiniz");
                return View(_mapper.Map<AdvancePaymentRejectedUpdateVm>(result.Data));
            }
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<AdvancePaymentRejectedUpdateVm>(result.Data));
            }

            return View(_mapper.Map<AdvancePaymentRejectedUpdateVm>(result.Data));
        }
        [HttpPost]
        public async Task<IActionResult> RejectedResponse(AdvancePaymentRejectedUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var AdvancePaymentEditDto = _mapper.Map<AdvancePaymentRejectedUpdateDto>(model);
            AdvancePaymentEditDto.ReturnStatus = ReturnStatus.Rejected;
            var result = await _advancePaymentService.UpdateRecetedAsync(AdvancePaymentEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
           var employee=  await _employeeService.GetByIdAsync(result.Data.EmployeeId);
           var identityuser = await _userManager.FindByIdAsync(employee.Data.IdentityId.ToString());
            await _emailService.SendMail($"{result.Data.CreatedDate} - Tarihinizdeki avans isteğiniz", $"{result.Data.CreatedDate} Tarihinizdeki {result.Data.AdvancePrice} miktarındaki avans isteğiniz Olumsuz olarak cevaplanmıştır ", identityuser.Email);
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
    }
}
