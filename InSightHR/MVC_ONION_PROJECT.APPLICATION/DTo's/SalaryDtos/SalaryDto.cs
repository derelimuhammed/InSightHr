using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.SalaryDtos
{
    public class SalaryDto
    {
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime SalaryDate { get; set; }
        public DateTime SalaryEndDate { get; set; }
        public double Salary { get; set; }
        public SalaryStatus SalaryStatus { get; set; }
    }
}
