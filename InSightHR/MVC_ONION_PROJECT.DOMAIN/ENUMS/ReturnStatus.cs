using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MVC_ONION_PROJECT.DOMAIN.ENUMS
{
    public enum ReturnStatus
    {
        [Display(Name = "İade Edildi")]
        Returned = 1,

        [Display(Name = "Atandı")]
        Assigned = 2,

        [Display(Name = "Reddedildi")]
        Rejected = 3,

        [Display(Name = "Beklemede")]
        Pending = 4
    }
}
