using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.AdvancePaymentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.ReimbursementDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.AddImageServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.ReimbursementService;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeAdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.EmployeeReimbursementVms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Controllers
{
    public class EmployeeReimbursementController : EmployeeBaseController
    {
        private readonly IReimbursementService _reimbursementService;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IEmployeeService _employeeService;
        private readonly IEmailService _emailService;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IAddImageService _addImageService;
        private readonly IEmailHelper _emailHelper;

        public EmployeeReimbursementController(IReimbursementService reimbursementService, IMapper mapper, IHttpContextAccessor contextAccessor, IEmployeeService employeeService, IEmailService emailService, SignInManager<IdentityUser> signInManager, IAddImageService addImageService, IEmailHelper emailHelper)
        {
            _reimbursementService = reimbursementService;
            _mapper = mapper;
            _contextAccessor = contextAccessor;
            _employeeService = employeeService;
            _emailService = emailService;
            _signInManager = signInManager;
            _addImageService = addImageService;
            _emailHelper = emailHelper;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _reimbursementService.GetAllAsync();
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<List<EmployeeReimbursementListVm>>(result.Data));
            }
            var identityId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userReimbursementListVM = await _reimbursementService.GetListOfMineReimbursement(identityId);
            return View(_mapper.Map<List<EmployeeReimbursementListVm>>(userReimbursementListVM.Data));
        }

        [HttpGet]
        public IActionResult AddReimbursement()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddReimbursement(EmployeeReimbursementCreateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var employee = await _employeeService.GetByidentityIdAsync(Guid.Parse(userId));
            model.EmployeeId = employee.Data.Id;
            model.PaymentStatus = DOMAIN.ENUMS.PaymentStatus.Pending;
            var pathname = await _addImageService.AddImage(model.ExpenseAttachmentsfile);
            if (pathname==null)
            {
                ErrorNoty("Lütfen Fatura Resmi Ekleyin");
                return View(model);
            }
            model.ExpenseAttachmentspath = pathname;
            var addResult = await _reimbursementService.AddAsync(_mapper.Map<ReimbursementCreateDto>(model));
            if (!addResult.IsSuccess)
            {
                ErrorNoty(addResult.Message);
                return View(model);
            }
            var mail = await _emailHelper.HarcamaMailIstek(employee.Data.Name, employee.Data.Surname, addResult.Data.Amount);

            await _emailService.SendMail("Harcama Talebi Hk.", mail, "cem.sener@bilgeadamboost.com");
            SuccessNoty(addResult.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _reimbursementService.DeleteAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return RedirectToAction(nameof(Index));
            }
            SuccessNoty(result.Message);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> EditReimbursement(Guid id)
        {
            var result = await _reimbursementService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeReimbursementUpdateVm>(result.Data));
            }
            if (result.Data.PaymentStatus != DOMAIN.ENUMS.PaymentStatus.Pending)
            {
                ErrorNoty("Cevaplanmış talep değiştirilemez.");
                return RedirectToAction(nameof(Index));
            }
            var categoryEditVm = _mapper.Map<EmployeeReimbursementUpdateVm>(result.Data);
            return View(categoryEditVm);
        }
        [HttpPost]

        public async Task<IActionResult> EditReimbursement(EmployeeReimbursementUpdateVm model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var categoryeditdto = _mapper.Map<ReimbursementUpdateDto>(model);
            categoryeditdto.PaymentStatus = DOMAIN.ENUMS.PaymentStatus.Pending;
            var pathname = await _addImageService.AddImage(model.ExpenseAttachmentsfile);
            if (pathname == null)
            {
                ErrorNoty("Lütfen Fatura Resmi Ekleyin");
                return View(model);
            }
            categoryeditdto.ExpenseAttachmentspath= pathname;
            var result = await _reimbursementService.UpdateAsync(categoryeditdto);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(model);
            }
            SuccessNoty(result.Message);
            var resultVM = _mapper.Map<EmployeeReimbursementUpdateVm>(result.Data);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DetailsReimbursement(Guid id)
        {
            var result = await _reimbursementService.GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                ErrorNoty(result.Message);
                return View(_mapper.Map<EmployeeReimbursementDetailsVm>(result.Data));
            }
            SuccessNoty(result.Message);
            var paymentDetailVm = _mapper.Map<EmployeeReimbursementDetailsVm>(result.Data);
            return View(paymentDetailVm);
        }
    }
}
