using MVC_ONION_PROJECT.APPLICATION.DTo_s.EmployeeDtos;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms
{
    public class TimeOffListVm
    {
        [Display(Name = "Kimlik")]
        public Guid Id { get; set; }

        [Display(Name = "Başlangıç Zamanı")]
        public DateTime StartTime { get; set; }

        [Display(Name = "Bitiş Zamanı")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Gün Sayısı")]
        public int NumberOfDays { get; set; }

        [Display(Name = "İzin Türü")]
        public string TimeOffTypeName { get; set; }

        public string TimeOffTypeId { get; set; }

        [Display(Name = "Çalışan")]
        public EmployeeDto Employee { get; set; }

        [Display(Name = "Oluşturan")]
        public string CreatedBy { get; set; }

        [Display(Name = "Zaman Aralığı Günü")]
        public int? TimeSpanDay { get; set; }

        [Display(Name = "Dönüş Durumu")]
        public ReturnStatus ReturnStatus { get; set; }

        [Display(Name = "Oluşturulma Tarihi")]
        public DateTime CreatedDate { get; set; }
    }
}
