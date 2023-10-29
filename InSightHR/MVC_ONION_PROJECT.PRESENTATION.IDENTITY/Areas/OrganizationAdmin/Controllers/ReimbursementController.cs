using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.EnumHelpers;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers
{
    public class ReimbursementController : OrganizationAdminBaseController
    {
        private readonly IReimbursementService _reimbursementService;
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEnumHelperService _enumHelper;
        private readonly IEmailHelper _emailHelper;


        public ReimbursementController(IReimbursementService reimbursementService, IMapper mapper, IEmployeeService employeeService, IEmailService emailService, UserManager<IdentityUser> userManager, IEnumHelperService enumHelper, IEmailHelper emailHelper)
        {
            _reimbursementService = reimbursementService;
            _mapper = mapper;
            _employeeService = employeeService;
            _emailService = emailService;
            _userManager = userManager;
            _enumHelper = enumHelper;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _reimbursementService.GetAllAsync();

            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<ReimbursementListVm>>(result.Data));
            }
            var filterResult = result.Data;
            var model = _mapper.Map<List<ReimbursementListVm>>(filterResult);
            foreach (var item in model)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }
            return View(model);
        }
        [HttpGet("OrganizationAdmin/Reimbursement/GetFilter/{filter?}")]
        public async Task<IActionResult> GetFilter(string filter)
        {
            var result = await _reimbursementService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<ReimbursementListVm>>(result.Data));
            }
            var model = _mapper.Map<List<ReimbursementListVm>>(result.Data);
            foreach (var item in model)
            {
                var employee = await _employeeService.GetByIdAsync(item.EmployeeId);
                item.Employee = employee.Data;
            }
            if (filter == "Hepsi")
                return PartialView("_ReimbursementListPartial", model);
            var filtermodel = model.FindAll(x => _enumHelper.GetDisplayName(x.PaymentStatus) == filter).ToList();
            SuccessNoty(result.Message);
            return PartialView("_ReimbursementListPartial", filtermodel);
        }
        [HttpPost("OrganizationAdmin/Reimbursement/SetReimbursement/{Id}/{returnStatusstring}")]
        public async Task SetReimbursement(string Id, string returnStatusstring)
        {
            PaymentStatus? returnStatus = null;
            foreach (PaymentStatus status in Enum.GetValues(typeof(PaymentStatus)))
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
            var reimbursement = await _reimbursementService.GetByIdAsync(Guid.Parse(Id));
            if (!ModelState.IsValid)
            {
                return;
            }
            var reimbursementEditdto = _mapper.Map<ReimbursementUpdateDto>(reimbursement.Data);
            reimbursementEditdto.PaymentStatus = returnStatus ?? PaymentStatus.Rejected;
            var result = await _reimbursementService.UpdateAsync(reimbursementEditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return;
            }
            if (reimbursement.Data.PaymentStatus != returnStatus)
            {
                var employee = await _employeeService.GetByIdAsync(result.Data.EmployeeId);
                var identityuser = await _userManager.FindByIdAsync(employee.Data.IdentityId.ToString());

                var mail = await _emailHelper.HarcamaGuncellemeMail(result.Data.CreatedDate, result.Data.Amount, returnStatus);
                await _emailService.SendMail($"{result.Data.PaymentStatus} - Tarihinizdeki Masraf isteğiniz", mail, identityuser.Email);
            }
            return;
        }
        [HttpGet("OrganizationAdmin/Reimbursement/RejectedResponse/{Id}")]
        public async Task<IActionResult> ReimbursementRejectedResponse(string Id)
        {
            var result = await _reimbursementService.GetByIdAsync(Guid.Parse(Id));
            if (result.Data.PaymentStatus == PaymentStatus.Rejected)
            {
                ErrorNoty("Reddedilmiş bir izni tekrardan rededemezsiniz");
                return View(_mapper.Map<ReimbursementRejectedUpdateVm>(result.Data));
            }
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<ReimbursementRejectedUpdateVm>(result.Data));
            }

            return View(_mapper.Map<ReimbursementRejectedUpdateVm>(result.Data));
        }
        [HttpPost]
        public async Task<IActionResult> RejectedResponse(ReimbursementRejectedUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var ReimbursementEditDto = _mapper.Map<ReimbursementRejectedUpdateDto>(model);
            ReimbursementEditDto.PaymentStatus = PaymentStatus.Rejected;
            var result = await _reimbursementService.UpdateRecetedAsync(ReimbursementEditDto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            var employee = await _employeeService.GetByIdAsync(result.Data.EmployeeId);
            var identityuser = await _userManager.FindByIdAsync(employee.Data.IdentityId.ToString());
            var returnStatus = ReimbursementEditDto.PaymentStatus;
            var mail = await _emailHelper.HarcamaGuncellemeMail(result.Data.CreatedDate, result.Data.Amount, returnStatus);
            await _emailService.SendMail($"{result.Data.CreatedDate} - Tarihinizdeki Masraf isteğiniz", mail, identityuser.Email);
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DetailsReimbursement(Guid id)
        {
            var result = await _reimbursementService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<ReimbursementDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var paymentDetailVm = _mapper.Map<ReimbursementDetailsVm>(result.Data);
            return View(paymentDetailVm);
        }
    }
}