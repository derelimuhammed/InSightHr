using Microsoft.AspNetCore.Identity;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using AutoMapper;
using MVC_Onion_Project.Application.Services.AccountService;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using MVC_ONION_PROJECT.APPLICATION.DTo_s;
using Microsoft.EntityFrameworkCore;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.APPLICATION.Services.EmailServices;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using System.Globalization;
using MVC_ONION_PROJECT.APPLICATION.Services.PasswordCreateServices;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeDebitService;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Reflection;

namespace MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IPasswordCreateService _passwordCreateService;
        private readonly IEmployeeDebitService _employeeDebitService;
        private readonly IMapper _mapper;
        private readonly IAccountService _accountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public EmployeeService(IEmployeeRepository employeeRepository, IMapper mapper, IAccountService accountService, IPasswordCreateService passwordCreateService, UserManager<IdentityUser> userManager, IEmployeeDebitService employeeDebitService, IDepartmentRepository departmentRepository, IHttpContextAccessor contextAccessor)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
            _accountService = accountService;
            _passwordCreateService = passwordCreateService;
            _userManager = userManager;
            _employeeDebitService = employeeDebitService;
            _departmentRepository = departmentRepository;
            _contextAccessor = contextAccessor;
            _userManager = userManager;
        }

        public async Task<IDataResult<EmployeeDto>> AddAsync(EmployeeCreateDto employeeCreateDto)
        {

            if (await _accountService.AnyAsync(x => x.Email == employeeCreateDto.Email))
            {
                return new ErrorDataResult<EmployeeDto>("kullanıcı zaten kayıtlı");
            }
            var user = await _userManager.FindByNameAsync(employeeCreateDto.Name + "." + employeeCreateDto.Surname);
            if (user != null)
            {
                employeeCreateDto.Surname = await DublicateIdentityUserAsync(employeeCreateDto.Name,employeeCreateDto.Surname);
                employeeCreateDto.Isduplicate = true;
            }
            IdentityUser identityUser = new();
            if (employeeCreateDto.IsCustomMail)
            {
                identityUser = new()
                {
                    Email = ConvertTurkishToEnglish(employeeCreateDto.Name + "." + employeeCreateDto.Surname + "@bilgeadamboost.com"),
                    EmailConfirmed = false,
                    UserName = ConvertTurkishToEnglish(employeeCreateDto.Name + "." + employeeCreateDto.Surname)

                };
            }
            else
            {
                identityUser = new()
                {
                    Email = ConvertTurkishToEnglish(employeeCreateDto?.Email),
                    EmailConfirmed = false,
                    UserName = ConvertTurkishToEnglish(employeeCreateDto.Name + "." + employeeCreateDto.Surname)
                };
            }


            DataResult<EmployeeDto> result = new ErrorDataResult<EmployeeDto>();
            var strategy = await _employeeRepository.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                using var transactionScope = await _employeeRepository.BeginTransactionAsync().ConfigureAwait(false);
                try
                {
                    var randomPas = await _passwordCreateService.GeneratePasswordAsync(8);//bogus.Random.Word();//şifreyi görmek için yapıldı

                    var identityResult = await _accountService.CreateUserAsync(identityUser, randomPas, employeeCreateDto.Role);
                    if (!identityResult.Succeeded)
                    {
                        result = new ErrorDataResult<EmployeeDto>(identityResult.ToString());
                        transactionScope.Rollback();
                        return;
                    }
                    var employee = _mapper.Map<Employee>(employeeCreateDto);
                    if (employeeCreateDto.Photopath != null)
                        employee.Photo = File.ReadAllBytes(employeeCreateDto.Photopath);
                    else
                    {
                        if (employeeCreateDto.GenderStatus == GenderStatus.Kadın)
                            employee.Photo = File.ReadAllBytes($"{Environment.CurrentDirectory}/wwwroot/uploads/BaseProfilePic/User-Female.jpg");
                        if (employeeCreateDto.GenderStatus == GenderStatus.Erkek)
                            employee.Photo = File.ReadAllBytes($"{Environment.CurrentDirectory}/wwwroot/uploads/BaseProfilePic/User-Male.png");
                    }
                    employee.IdentityId = identityUser.Id;
                    employee.Email = identityUser.NormalizedEmail;
                    employee.IsActive = true;
                    await _employeeRepository.AddAsync(employee);
                    await _employeeRepository.SaveChangesAsync();
                    result = new SuccessDataResult<EmployeeDto>(_mapper.Map<EmployeeDto>(employee), "Employee ekleme başarılı");
                    result.Data.Password = randomPas;
                    transactionScope.Commit();
                }
                catch (Exception ex)
                {
                    result = new ErrorDataResult<EmployeeDto>($"Ekleme başarısız- {ex.Message}");
                    transactionScope.Rollback();

                }
                finally
                {
                    transactionScope.Dispose();

                }

            });
            return result;
        }
        public async Task<IDataResult<List<EmployeeListDto>>> GetAllAsync()
        {
            //var categories = await _categoryRepository.GetAllAsync(x => x.CreatedDate >= DateTime.Now.AddDays(-2));
            var Employes = await _employeeRepository.GetAllAsync();
            if (Employes.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDto>>("kullanıcı bulunamadı.");
            }
            var employeesListDto = _mapper.Map<List<EmployeeListDto>>(Employes);
            return new SuccessDataResult<List<EmployeeListDto>>(employeesListDto, "Listeleme başarılı");
        }
        public async Task<IDataResult<EmployeeDto>> UpdateAsync(EmployeeUpdateDto employeeEditDTO)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeEditDTO.Id);
            if (employee == null)
            {
                return new ErrorDataResult<EmployeeDto>("çalışan bulunamadı.");

            }
            var employees = await _employeeRepository.GetAllAsync();
            var newEmployees = employees.ToList();
            newEmployees.Remove(employee);
            if (employeeEditDTO.Photopath != null)
            {
                employee.Photo = File.ReadAllBytes(employeeEditDTO?.Photopath);
            }
            var updatedEmployee = _mapper.Map(employeeEditDTO, employee);
            await _employeeRepository.UpdateAsync(updatedEmployee);
            await _employeeRepository.SaveChangesAsync();
            return new SuccessDataResult<EmployeeDto>(_mapper.Map<EmployeeDto>(updatedEmployee), "Departman güncelleme başarılı");


        }
        public async Task<IDataResult<EmployeeDto>> GetByIdAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
                return new ErrorDataResult<EmployeeDto>("Çalışan bulunamadı.");
            var employeeDTO = _mapper.Map<EmployeeDto>(employee);
            return new SuccessDataResult<EmployeeDto>(employeeDTO, "Giriş Başarılı");
        }
        public async Task<IDataResult<EmployeeDto>> GetByidentityIdAsync(Guid identityId)
        {
            var employee = await _employeeRepository.GetAsync(x => x.IdentityId == identityId.ToString(), false);
            if (employee == null)
                return new ErrorDataResult<EmployeeDto>("Çalışan bulunamadı.");
            var employeeDTO = _mapper.Map<EmployeeDto>(employee);
            return new SuccessDataResult<EmployeeDto>(employeeDTO, "Belirtilen çalışan var.");
        }
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee is null)
                return new ErrorResult("Çalışan Bulunamadi");
            var identityUser = await GetByidentityIdAsync(Guid.Parse(employee.IdentityId));
            if (identityUser is null)
                return new ErrorResult("Çalışan Bulunamadi");
            var debits = await _employeeDebitService.GetAllAsync();
            var employeeDebit = debits.Data.Where(x => x.EmployeeId == employee.Id && (x.ReturnStatus == ReturnStatus.Assigned || x.ReturnStatus == ReturnStatus.Pending)).FirstOrDefault();
            if (employeeDebit is null)
            {
                var result = await _accountService.DeleteUserAsync(identityUser.Data.IdentityId.ToString());
                if (!result.Succeeded)
                    return new ErrorResult(result.Errors.First().Description);
                await _employeeRepository.DeleteAsync(employee);
                await _employeeRepository.SaveChangesAsync();
                return new SuccessResult("Çalışan silme işlemi başarılı");
            }
            else
            {
                return new ErrorResult("Çalışana bağlı zimmet bulunmaktadır. Zimmeti kaldırmadan çalışanı silemezsiniz");
            }

        }
        public async Task<IDataResult<DepartmentDto>> GetEmployeeOrganization(Guid identityId)
        {
            var employee = await _employeeRepository.GetAllAsync(x => x.IdentityId == identityId.ToString(), false);
            if (employee == null)
                return new ErrorDataResult<DepartmentDto>("Çalışan bulunamadı.");
            var department = await _departmentRepository.GetByIdAsync(employee.First().DepartmentId);
            if (department == null)
            {
                return new ErrorDataResult<DepartmentDto>("Department bulunamadı.");
            }
            var DepartmentDto = _mapper.Map<DepartmentDto>(department);
            return new SuccessDataResult<DepartmentDto>(DepartmentDto, "Belirtilen çalışan var.");
        }

        private class PasswordGenerator
        {

            public static string GenerateRandomPassword(int length)
            {
                string characters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                Random rnd = new Random();
                char[] password = new char[length];
                for (int i = 0; i < length; i++)
                {
                    password[i] = characters[rnd.Next(characters.Length)];
                }
                return new string(password);
            }
        }
        private string ConvertTurkishToEnglish(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Türkçe karakterleri İngilizce karakterlere dönüştür
            string normalized = input.Normalize(NormalizationForm.FormKD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    builder.Append(c);
            }

            // Boşlukları tire ile değiştir
            string result = builder.ToString().Replace(" ", "-");

            // Küçük harf yap ve döndür
            return result.ToLowerInvariant();
        }
        public async Task<IDataResult<List<EmployeeListDto>>> GetEmployeesInOrganization()
        {
            //var categories = await _categoryRepository.GetAllAsync(x => x.CreatedDate >= DateTime.Now.AddDays(-2));
            var Employes = await _employeeRepository.GetAllAsync();
            var OrganizationEmployee = Employes.Where(x => x.Department.OrganizationId == Guid.Parse(_contextAccessor.HttpContext.Session.GetString("OrganizationId")));
            if (OrganizationEmployee.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDto>>("Çalışan bulunamadı.");
            }
            var employeesListDto = _mapper.Map<List<EmployeeListDto>>(OrganizationEmployee);
            return new SuccessDataResult<List<EmployeeListDto>>(employeesListDto, "Listeleme başarılı");
        }

        public async Task<IDataResult<List<EmployeeListDto>>> GetEmployeesInOrganization(Guid organizationId)
        {
            //var categories = await _categoryRepository.GetAllAsync(x => x.CreatedDate >= DateTime.Now.AddDays(-2));
            var Employes = await _employeeRepository.GetAllAsync();
            var OrganizationEmployee = Employes.Where(x => x?.Department?.OrganizationId == organizationId);
            if (OrganizationEmployee.Count() <= 0)
            {
                return new ErrorDataResult<List<EmployeeListDto>>("Çalışan bulunamadı.");
            }
            var employeesListDto = _mapper.Map<List<EmployeeListDto>>(OrganizationEmployee);
            return new SuccessDataResult<List<EmployeeListDto>>(employeesListDto, "Listeleme başarılı");
        }
        public async Task<string> DublicateIdentityUserAsync(string Name,string Surname)
        {
            string SurName = "";
            IdentityUser? user = await _userManager.FindByNameAsync(Name+"."+Surname);
            // Aynı kullanıcı adı mevcutsa, farklı bir kullanıcı adı oluşturun
            if (user!=null)
            {
                
                int counter = 1;
               
                while (user != null)
                {
                    SurName = $"{Surname}{counter}";
                    user = await _userManager.FindByNameAsync(Name+"."+SurName);
                    counter++;
                }
            }
            return SurName;
        }
    }
}