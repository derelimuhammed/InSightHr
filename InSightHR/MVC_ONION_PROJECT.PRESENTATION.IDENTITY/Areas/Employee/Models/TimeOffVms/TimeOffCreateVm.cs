using Microsoft.AspNetCore.Mvc.Rendering;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.Employee.Models.TimeOffVms
{
    public class TimeOffCreateVm
    {

        [DisplayName("Başlangıç Tarihi")]
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }

        [DisplayName("Bitiş Tarihi")]

        [DataType(DataType.Date)]
        public DateTime EndTime { get; set; }
        public int NumberOfDays { get; set; }
        [DisplayName("İzin Nedeni")]
        public string Reason { get; set; }

        [DisplayName("İzin Türü")]
        public string TimeOffTypeId { get; set; }
        public SelectList? TimeOffList { get; set; }
        public string CreatedBy { get; set; }
        public ReturnStatus ReturnStatus { get; set; }
        public Guid EmployeeId { get; set; }
    }
}
