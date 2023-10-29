using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDebitDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.OrgAssetDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos;
using MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService;
using MVC_ONION_PROJECT.APPLICATION.Services.EmployeeServicces;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;

using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.SalaryService
{
    public class SalaryService : ISalaryService
    {
        private readonly IEmployeeSalaryRepository _employeeSalaryRepository;
		private readonly IMapper _mapper;
		private readonly IDepartmentService _departmentService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IEmployeeService _employeeService;

		public SalaryService(IEmployeeSalaryRepository employeeSalaryRepository, IMapper mapper, IDepartmentService departmentService, IHttpContextAccessor httpContextAccessor, IEmployeeService employeeService)
		{
			_employeeSalaryRepository = employeeSalaryRepository;
			_mapper = mapper;
			_departmentService = departmentService;
			_httpContextAccessor = httpContextAccessor;
			_employeeService = employeeService;
		}
		public async Task<IDataResult<SalaryDto>> AddAsync(SalaryCreateDto salaryCreateDto)
        {
            var employeeSalary = _mapper.Map<EmployeeSalary>(salaryCreateDto);
            var IsSameSalaryExist = await _employeeSalaryRepository.GetAsync(x => x.EmployeeId == salaryCreateDto.EmployeeId && x.Salary == salaryCreateDto.Salary);
            if (IsSameSalaryExist != null)
            {
                return new ErrorDataResult<SalaryDto>("Bu çalışan için girilen maaş bilgisi mevcut");
            }
            var employeeSalaries = await _employeeSalaryRepository.GetAllAsync();
            if (employeeSalaries != null)
            {
                var thisEmployeeSalaries = employeeSalaries.Where(x=>x.EmployeeId== salaryCreateDto.EmployeeId).ToList();
                foreach (var salary in thisEmployeeSalaries)
                {
                    if((salaryCreateDto.SalaryDate>= salary.SalaryDate && salaryCreateDto.SalaryDate <= salary.SalaryEndDate) || (salaryCreateDto.SalaryEndDate >= salary.SalaryDate && salaryCreateDto.SalaryEndDate <= salary.SalaryEndDate))
                    {
                        return new ErrorDataResult<SalaryDto>("Seçtiğiniz tarih aralığı için maaş bilgisi mevcut. Lütfen tarhileri kontrol ediniz.");
                    }
                }
            }
            employeeSalary.SalaryStatus = SalaryStatus.Current;
            //Çalışanın önceki maaş bilgisini pasife çekiyoruz
            var getPreviousSalary = await _employeeSalaryRepository.GetAsync(x => x.EmployeeId == salaryCreateDto.EmployeeId && x.Status != Status.Deleted && x.SalaryStatus != SalaryStatus.Previous);
            if (getPreviousSalary != null)
            {
                getPreviousSalary.SalaryStatus = SalaryStatus.Previous;
                await _employeeSalaryRepository.UpdateAsync(getPreviousSalary);
                await _employeeSalaryRepository.SaveChangesAsync();
            }         
            await _employeeSalaryRepository.AddAsync(employeeSalary);
            await _employeeSalaryRepository.SaveChangesAsync();
            return new SuccessDataResult<SalaryDto>(_mapper.Map<SalaryDto>(employeeSalary), "Çalışan maaş bilgisi başarıyla eklendi");

        }

        public async Task<IResult> DeleteAsync(Guid id)
        {
            var employeeSalary = await _employeeSalaryRepository.GetByIdAsync(id);
            if (employeeSalary == null)
            {
                return new ErrorResult("Sistemde çalışan için seçilen maaş bilgisi bulunamadı.");
            }
            else
            {
                await _employeeSalaryRepository.DeleteAsync(employeeSalary);
                await _employeeSalaryRepository.SaveChangesAsync();
                return new SuccessResult("Çalışan için seçilen maaş bilgisi başarıyla silindi");
            }
        }

        public async Task<IDataResult<List<SalaryListDto>>> GetAllAsync()
        {
            var employeeSalaryList = await _employeeSalaryRepository.GetAllAsync();
            if (employeeSalaryList.Count() <= 0)
            {
                return new ErrorDataResult<List<SalaryListDto>>("Sistemde çalışan maaş bilgisi bulunamadı.");
            }
            var employeeSalaryListDto = _mapper.Map<List<SalaryListDto>>(employeeSalaryList);
            return new SuccessDataResult<List<SalaryListDto>>(employeeSalaryListDto, "Çalışan maaş bilgisi listeleme başarılı");
        }

	
		public async Task<IDataResult<List<SalaryListDto>>> GetAllCurrentAsync()
        {
            var employeeSalaryList = await _employeeSalaryRepository.GetAllAsync();
            if (employeeSalaryList.Count() <= 0)
            {
                return new ErrorDataResult<List<SalaryListDto>>("Sistemde çalışan maaş bilgisi bulunamadı.");
            }
            var newEmployeeSalaryList= employeeSalaryList.Where(x=>x.SalaryStatus==SalaryStatus.Current).ToList();

			var orgEmployeeList = await _departmentService.GetEmployeesInOrganization();
            if (!orgEmployeeList.IsSuccess)
            {
                return new ErrorDataResult<List<SalaryListDto>>("Sistemde çalışan maaş bilgisi bulunamadı.");
            }
            var employeeList = orgEmployeeList.Data.ToList();
			List<EmployeeSalary> employeeSalaries = new List<EmployeeSalary>();
			foreach (var orgEmployee in employeeList)
			{
				var employeeSalary = newEmployeeSalaryList.Where(x => x.EmployeeId == orgEmployee.Id); //çalışanın maaşı
				employeeSalaries.AddRange(employeeSalary); // Organizasyondaki çalışanların maaşları
			}
			if (employeeSalaries.Count() == 0)
			{
				return new ErrorDataResult<List<SalaryListDto>>("Sistemde maaş kaydı Yok");
			}
			var employeeSalaryListDto = _mapper.Map<List<SalaryListDto>>(employeeSalaries);
            return new SuccessDataResult<List<SalaryListDto>>(employeeSalaryListDto, "Çalışan maaş bilgisi listeleme başarılı");
        }


        public async Task<IDataResult<List<SalaryListDto>>> GetByEmployeeIdAsync(Guid employeeId)
        {
            var SalaryList = await _employeeSalaryRepository.GetAllAsync();
            var employeeSalaryList= SalaryList.Where(x=>x.EmployeeId== employeeId).ToList();
            if (employeeSalaryList.Count() <= 0)
            {
                return new ErrorDataResult<List<SalaryListDto>>("Sistemde çalışan maaş bilgisi bulunamadı.");
            }
            var employeeSalaryListDto = _mapper.Map<List<SalaryListDto>>(employeeSalaryList);
            return new SuccessDataResult<List<SalaryListDto>>(employeeSalaryListDto, "Çalışan maaş bilgisi listeleme başarılı");
        }

        public async Task<IDataResult<SalaryDto>> GetByIdAsync(Guid? id)
        {
            var employeeSalary = await _employeeSalaryRepository.GetByIdAsync(id);
            if (employeeSalary == null)
            {
                return new ErrorDataResult<SalaryDto>("Belirtilen ID ile maaş kaydı bulunamadı.");
            }
            var employeeSalaryDTO = _mapper.Map<SalaryDto>(employeeSalary);

            return new SuccessDataResult<SalaryDto>(employeeSalaryDTO, "Belirtilen ID'de maaş kaydı mevcut.");
        }

        public async Task<IDataResult<SalaryDto>> UpdateAsync(SalaryUpdateDto salaryEditDto)
        {
            var employeeSalary = await _employeeSalaryRepository.GetByIdAsync(salaryEditDto.Id);
            if (employeeSalary == null)
            {
                return new ErrorDataResult<SalaryDto>("Çalışana ait böyle bir maaş bilgisi bulunamadı.");

            }
            var employeeSalaries = await _employeeSalaryRepository.GetAllAsync();
            var newEmployeeSalaries = employeeSalaries.ToList();
            newEmployeeSalaries.Remove(employeeSalary);
            var hasEmployeeSalary = newEmployeeSalaries.Any(x => x.Salary == salaryEditDto.Salary && x.SalaryDate== salaryEditDto.SalaryDate);
            if (hasEmployeeSalary)
            {
                return new ErrorDataResult<SalaryDto>("Çalışana ait bu maaş bilgisi zaten kayıtlı");
            }
            var updatedEmployeeSalary = _mapper.Map(salaryEditDto, employeeSalary);
            await _employeeSalaryRepository.UpdateAsync(updatedEmployeeSalary);
            await _employeeSalaryRepository.SaveChangesAsync();
            return new SuccessDataResult<SalaryDto>(_mapper.Map<SalaryDto>(updatedEmployeeSalary), "Çalışan maaş bilgisi güncelleme başarılı");
        }
    }
}
