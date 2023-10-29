using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.DepartmentDtos
{
    public class DepartmentUpdateDto
    {
        public Guid Id { get; set; }
        public string DepartmentName { get; set; }
        public string? Description { get; set; }
    }
}
