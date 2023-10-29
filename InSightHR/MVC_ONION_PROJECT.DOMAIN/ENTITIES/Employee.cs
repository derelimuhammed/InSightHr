using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class Employee : AuditableEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public GenderStatus GenderStatus { get; set; }
        public string? Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? RecruitmentDate { get; set; }
        public Guid? DepartmentId { get; set; }
        public Byte[]? Photo { get; set; }
        public string? Photopath { get; set; }
        public Role Role { get; set; }
        [DefaultValue(true)]
        public bool IsActive { get; set; }
        public string IdentityId { get; set; }
        //navigation prop
        public virtual Department? Department { get; set; }
        public virtual ICollection<EmployeeSalary> EmployeeSalary { get; set; }
        public virtual ICollection<AdvancePayment> AdvancePayments { get; set; }
        public virtual ICollection<EmployeeDebit> EmployeeDebit { get; set; }
        public virtual ICollection<TimeOff?> TimeOff { get; set; }
        public virtual ICollection<Reimbursement?> Reimbursement { get; set; }
    }
}
