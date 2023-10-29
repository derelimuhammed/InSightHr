using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.AdvanceVms;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.ReimbursementVm;
using MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.TimeOffVms;

namespace MVC_ONION_PROJECT.PRESENTATION.IDENTITY.Areas.OrganizationAdmin.Models.IndexVms
{
    public class IndexVm
    {
        public List<ReimbursementListVm>? ReimbursementListVmTake5 { get; set; }
        public List<AdvanceListVm>? AdvanceListVmTake5 { get; set; }
        public List<TimeOffListVm>? TimeOffListVmTake5 { get; set; }
        public int EmployeeCount { get; set; }
        public int NumberOfPeopleOnLeave { get; set; }
    }
}
