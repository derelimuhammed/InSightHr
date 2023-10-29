using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos
{
    public class EmployeeListDto
    {
        public Guid Id { get; set; }
        public Byte[]? Photo { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public Guid IdentityId { get; set;}
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public Guid DepartmentId { get; set; }
        public string? Photopath { get; set; }
        public DateTime RecruitmentDate { get; set; }
        public GenderStatus GenderStatus { get; set; }
    }
}
