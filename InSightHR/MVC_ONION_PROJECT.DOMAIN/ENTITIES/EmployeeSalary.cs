using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.DOMAIN.ENTITIES
{
    public class EmployeeSalary : AuditableEntity
    {
        public Guid EmployeeId { get; set; }
        public DateTime SalaryDate { get; set; }
		public DateTime SalaryEndDate { get; set; }
		public double Salary { get; set; }    
        public SalaryStatus SalaryStatus { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
