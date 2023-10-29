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
    public enum AssetStatus
    {
        [Display(Name = "Atandı")]
        Assigned = 1,
        [Display(Name = "Atanmadı")]
        NotAssigned = 2,
        [Display(Name = "Onayda")]
        PendingApproval =3,
        [Display(Name = "İptal Edildi")]
        Rejected =4,
        [Display(Name = "Geri Alındı")]
        Returned =5
    }
}
