using MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos;
using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.Services.DepartmentService
{
    public interface IDepartmentService
    {
        public Task<IDataResult<DepartmentDto>> AddAsync(DepartmentCreateDto departmentCreateDto);
        Task<IResult> DeleteAsync(Guid id);
        Task<IDataResult<List<DepartmentListDto>>> GetAllAsync();
        Task<IDataResult<DepartmentDto>> GetByIdAsync(Guid? id);
        Task<int> GetByOrganizationCount(Guid? organizationId);
        Task<IDataResult<List<EmployeeDto>>> GetEmployeesInOrganization();
        Task<IDataResult<DepartmentDto>> UpdateAsync(DepartmentUpdateDto departmentEditDTO);

	}
}
