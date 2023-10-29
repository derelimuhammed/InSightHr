using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.APPLICATION.DTo_s.PackageDtos
{
    public class PackageDto
    {
        public  Guid Id { get; set; }
        public string PackageName { get; set; }
        public double Price { get; set; }
        public int NumberOfUser { get; set; }
        public double PackageDurationMonth { get; set; }
        public bool IsActive { get; set; }
    }
}
