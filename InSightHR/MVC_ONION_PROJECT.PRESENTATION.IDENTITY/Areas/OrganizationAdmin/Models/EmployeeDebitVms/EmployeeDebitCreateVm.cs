using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.EmployeeDebitVms
{
    public class EmployeeDebitCreateVm
    {
        [DisplayName("Zimmet Adı")]
        public Guid OrgAssetId { get; set; }
        public SelectList? DebitList { get; set; }
        [DisplayName("Çalışan Adı")]
        public Guid EmployeeId { get; set; }
        public SelectList? EmployeeList { get; set; }

        [DisplayName("Kategori Adı")]
        public Guid CategoryId { get; set; }
        public SelectList? CategoryList { get; set; }

        [DisplayName("Atama Tarihi")]
        [DataType(DataType.Date)]   
        public DateTime AssignmentDate { get; set; }

        [DisplayName("Dönüş Durumu")]
        public ReturnStatus ReturnStatus { get; set; }

    }
}
