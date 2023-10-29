using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVC_Onion_Project.Application.Services.AccountService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailHelper;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.APPLICATION.Services.OrganizationServices;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Controllers;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Vms;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Controllers
{

    public class AccountController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmployeeService _employeeService;
        private readonly IMapper _mapper;
        private readonly IEmailHelper _email;
        private readonly IOrganizationService _organizationService;
        private readonly IAccountService _accountService;
        private readonly IHttpContextAccessor _contextAccessor;



        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,IEmployeeService employeeService, IMapper mapper, IEmailHelper emailHelperService, IAccountService accountService, IOrganizationService organizationService, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _employeeService = employeeService;
            _mapper = mapper;
            _email = emailHelperService;
            _accountService = accountService;
            _organizationService = organizationService;
            _contextAccessor = contextAccessor;
        }
        public IActionResult Index()
        {
            return View();
        }




        public  IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }

            var user = await _userManager.FindByEmailAsync(loginVM.Email);

            if (user == null)
            {
                ErrorNoty("Email veya şifre hatalı");
                return View(loginVM);
            }
            var corfimmail = await _userManager.IsEmailConfirmedAsync(user);
            if (!corfimmail)
            {
                ErrorNoty("Lütfen mail Adresinizi onaylayın");
                return View(loginVM);
            }
            var userRole = await _userManager.GetRolesAsync(user);
            if (userRole == null)
            {
                ErrorNoty("Kullanıcıya ait rol bulunamadı");
                return View(loginVM);
            }
            var organizationId =await  _accountService.GetOrganizationdAsync(Guid.Parse(user.Id));
            Guid organizationguid = new Guid();
            if (!Guid.TryParse(organizationId, out organizationguid))
            {
                ErrorNoty("Kullanıcıya ait organizasyon bulunamadı");
                return View(loginVM);
            }
            var organization =await _organizationService.GetByIdAsync(organizationguid);
            if (!organization.Data.IsActive)
            {
                ErrorNoty("Organization aktif değil lütfen site yöneticisiyle iletişime geçin");
                return View(loginVM);
            }
            if (!(organization.Data.ContractEnd>DateTime.Now))
            {
                ErrorNoty("Lisansınız bitmiştir. lütfen site yöneticisiyle iletişime geçin");
                return View(loginVM);
            }
            var checkPass = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if (!checkPass.Succeeded)
            {
                ErrorNoty("Email veya şifre hatalı");
                return View(loginVM);

            }
            var result = await _employeeService.GetEmployeeOrganization(Guid.Parse(user.Id));
           
            if (!result.IsSuccess)
            {
                return View("Error");
            }
            var kullanıcıresult = await _employeeService.GetAllAsync();
            if (kullanıcıresult.IsSuccess)
            {
                var kullanıcı = kullanıcıresult.Data.FirstOrDefault(x => x.IdentityId.ToString() == user.Id);
                if ((kullanıcı is  null||kullanıcı?.IsActive == false))
                {
                    ErrorNoty("Kullanıcı Silinmiş veya Aktif Değildir Lütfen Organizasyon Yöneticinizle iletişime Geçin");
                    return View(loginVM);
                }
            }
            
            var id = user.Id;
            HttpContext.Session.SetString("OrganizationId", result.Data.OrganizationId.ToString());
            return RedirectToAction("index", "Home", new { Area = userRole[0].ToString(), userId = id});

        }

        [HttpGet]
        public async Task<IActionResult> VerifyEmail(string userId, string token)
        {
            var model = new VerifyEmailVm()
            {
                Id = userId,
                Token = token
            };
            return View(model);


        }
        [HttpPost]
        public async Task<IActionResult> VerifyEmail(VerifyEmailVm model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ErrorNoty("Kullanıcı bulunamadı");
                return View(model);
            }


            if (model.NewPassword == model.CorfirmPassword)
            {
                string decodedCallbackUrl = model.Token.Replace(" ", "+");
                var result = await _userManager.ConfirmEmailAsync(user, decodedCallbackUrl);

                if (result.Succeeded)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resultpas = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                    if (resultpas.Succeeded)
                    {
                        return RedirectToAction("login");


                    }
                    else
                    {
                        ErrorNoty(resultpas.Errors.First().Description);
                    }
                }
                else
                {
                    ErrorNoty("Eposta Doğrulaması Başarız");
                    return View(model);
                }
                return View(model);
            }
            else
            {
                ErrorNoty("Girilen şifreler uyumsuz");
                return View(model);
            }


        }


        // Şifre sıfırlama
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    await _email.SendVerificationEmailAsync("Şifre Yenileme", "Şifre Yenilemek için", "Account", "ResetPassword", user);
                    return RedirectToAction("Login");
                }

                ModelState.AddModelError(string.Empty, "Kullanıcı bulunamadı.");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string userId)
        {
            var model = new ResetPasswordViewModel()
            {
                Token = token
           ,
                Id = Guid.Parse(userId)
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(model.Id.ToString());
                if (user != null)
                {
                    if (model.NewPassword == model.CorfirmPassword)
                    {
                        string decodedCallbackUrl = model.Token.Replace(" ", "+");
                        var resetuser = await _userManager.ResetPasswordAsync(user, decodedCallbackUrl, model.NewPassword);
                        if (!resetuser.Succeeded)
                        {
                            ErrorNoty(resetuser.Errors.First().Description);
                            return RedirectToAction("Login");
                        }
                        SuccessNoty("Şifre Değiştirildi");
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ErrorNoty("Şifreler Uyumsuz");
                    }

                }
                else
                {
                    ErrorNoty("Kullanıcı bulunamadı.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var userId = _contextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId);
            var result = await _userManager.ChangePasswordAsync(user, vm.OldPassword, vm.NewPassword);
            if (!result.Succeeded)
            {
                ErrorNoty("Şifre Değiştirilemedi.");
                return View(vm);
            }
            SuccessNoty("Şifre Başarıyla Güncellendi.");
            return RedirectToAction(nameof(Index));
        }
    }
}
