using AutoMapper;
using Microsoft.AspNetCore.Http;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using MVC_ONION_PROJECT.DOMAIN.Utilities.Results;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService
{
    public class DepartmentService: IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, IEmployeeRepository employeeRepository)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _employeeRepository = employeeRepository;
        }

        public async Task<IDataResult<DepartmentDto>> AddAsync(DepartmentCreateDto departmentCreateDto)
        {
            var department = _mapper.Map<Department>(departmentCreateDto);
            var identityId = _httpContextAccessor?.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "User bulunamadı";
            var employee= await _employeeRepository.GetAsync(x=>x.IdentityId== identityId);
            if (employee!=null)
            {
                var usingdepartment =await _departmentRepository?.GetAllAsync();
                var result = usingdepartment.FirstOrDefault(x => x.Id == employee.DepartmentId);
                if (result != null)
                {
                    department.OrganizationId = Guid.Parse(_httpContextAccessor.HttpContext?.Session.GetString("OrganizationId"));
                }
            }

            await _departmentRepository.AddAsync(department); ;
            await _departmentRepository.SaveChangesAsync();
            return new SuccessDataResult<DepartmentDto>(_mapper.Map<DepartmentDto>(department), "Departman başarıyla eklendi");
        }
        public async Task<IDataResult<List<DepartmentListDto>>> GetAllAsync()
        {
            var usedOrganizationDepartment= _httpContextAccessor.HttpContext?.Session.GetString("OrganizationId");
            var departments = await _departmentRepository.GetAllAsync(x=>x.OrganizationId==Guid.Parse(usedOrganizationDepartment),false);
            if (departments.Count() <= 0)
            {
                return new ErrorDataResult<List<DepartmentListDto>>("Sistemde departman bulunamadı.");
            }
            var departmentListDto = _mapper.Map<List<DepartmentListDto>>(departments);
            return new SuccessDataResult<List<DepartmentListDto>>(departmentListDto, "Listeleme başarılı");
        }
        public async Task<IResult> DeleteAsync(Guid id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return new ErrorResult("Sistemde kategori bulunamadı.");
            }
            var employees = await _employeeRepository.GetAllAsync();
            var employeesInDepartment= employees.Where(x=>x.DepartmentId == id).ToList();
            if(employeesInDepartment.Count()<=0)
            {
                await _departmentRepository.DeleteAsync(department);
                await _departmentRepository.SaveChangesAsync();
                return new SuccessResult("Silme başarılı");
            }
            else
            {
                return new ErrorResult("Bu departmanda bağlı çalışanlar olduğu için silemezsiniz.");
            }

        }
        public async Task<IDataResult<DepartmentDto>> UpdateAsync(DepartmentUpdateDto departmentEditDTO)
        {
            var department = await _departmentRepository.GetByIdAsync(departmentEditDTO.Id);
            if (department == null)
            {
                return new ErrorDataResult<DepartmentDto>("departman bulunamadı.");

            }
            var departments = await _departmentRepository.GetAllAsync();
            var newDepartments = departments.ToList();
            newDepartments.Remove(department);
            var hasDepartment = newDepartments.Any(x => x.DepartmentName == departmentEditDTO.DepartmentName);
            if (hasDepartment)
            {
                return new ErrorDataResult<DepartmentDto>("departman zaten kayıtlı");
            }
            var updatedDepartment = _mapper.Map(departmentEditDTO, department);
            await _departmentRepository.UpdateAsync(updatedDepartment);
            await _departmentRepository.SaveChangesAsync();
            return new SuccessDataResult<DepartmentDto>(_mapper.Map<DepartmentDto>(updatedDepartment), "Departman güncelleme başarılı");

        }
       public async Task<IDataResult<DepartmentDto>> GetByIdAsync(Guid? id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return new ErrorDataResult<DepartmentDto>("Belirtilen ID ile Departmman bulunamadı.");

            }
            var departmentDTO = _mapper.Map<DepartmentDto>(department);

            return new SuccessDataResult<DepartmentDto>(departmentDTO, "Belirtilen ID'de Departmman var.");
        }
        public async Task<int> GetByOrganizationCount(Guid? organizationId)
        {
            var allDepartments =await _departmentRepository.GetAllAsync(x=>x.OrganizationId== organizationId, false); // Tüm departmanları alın
            var totalEmployees = 0;
            foreach (var department in allDepartments)
            {
                var employeesInDepartment =await _employeeRepository.GetAllAsync(x=>x.DepartmentId== department.Id,true); // Departmandaki çalışanları alın
                totalEmployees += employeesInDepartment.Count(); // Departmandaki çalışan sayısını toplam çalışan sayısına ekleyin
            }
            return totalEmployees;
        }
        public async Task<IDataResult<List<EmployeeDto>>> GetEmployeesInOrganization()
        {
            var usedOrganizationDepartment =Guid.Parse(_httpContextAccessor.HttpContext?.Session.GetString("OrganizationId"));
            var allDepartments = await _departmentRepository.GetAllAsync(x => x.OrganizationId == usedOrganizationDepartment, false); // Tüm departmanları alın organizayona bağlı
            List<Employee> employees = new List<Employee>();
            foreach (var department in allDepartments)
            {
                var employeesInDepartment = await _employeeRepository.GetAllAsync(x => x.DepartmentId == department.Id, true); // Departmandaki çalışanları alın
                employees.AddRange(employeesInDepartment); // Departmandaki çalışanları listeye alın
            }
            if (employees.Count()==0)
            {
                return new ErrorDataResult<List<EmployeeDto>>( "Organizasyona Bağlı Çalışan Yok");
            }
            return new SuccessDataResult<List<EmployeeDto>>(_mapper.Map<List<EmployeeDto>>(employees),"Organizasyona Bağlı Çalışanlar Listelendi");
        }

    }

}
