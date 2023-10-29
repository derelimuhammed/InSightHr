using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.OrganizationDtos
{
    public class OrganizationCreateDto
    {
        public string OrganizationName { get; set; }
        public string OrganizationEmail { get; set; }
        public string? OrganizationPhone { get; set; }
        public string? OrganizationAddress { get; set; }

        public string TaxNumber { get; set; }
        public IFormFile? Logopath { get; set; }
        public DateTime ContractStart { get; set; }
        public DateTime ContractEnd { get; set; }
        public Guid? PackageId { get; set; }
        public bool IsActive { get; set; }=true;
    }
}
