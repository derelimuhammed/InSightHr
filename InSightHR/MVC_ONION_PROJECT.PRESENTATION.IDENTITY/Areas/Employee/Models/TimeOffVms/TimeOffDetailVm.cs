using System.ComponentModel;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms
{
    public class TimeOffDetailVm
    {
        public Guid Id { get; set; }

        [DisplayName("Başlangıç Tarihi")]
        public DateTime StartTime { get; set; }

        [DisplayName("Bitiş Tarihi")]
        public DateTime EndTime { get; set; }

        [DisplayName("İzinli Gün Sayısı")]
        public int NumberOfDays { get; set; }

        [DisplayName("İzin Nedeni")]
        public string Reason { get; set; }

        [DisplayName("İzin Türü")]
        public string TimeOffTypeId { get; set; }

        [DisplayName("Çalışan Kimliği")]
        public string EmployeeId { get; set; }

    }
}
