using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MVC_ONION_PROJECT.DOMAIN.ENUMS
{
    public enum PaymentStatus
    {
        [Display(Name = "Beklemede")]
        Pending,
        [Display(Name = "Onaylandı")]
        Accepted,
        [Display(Name = "Reddedildi")]
        Rejected 
    }

}
