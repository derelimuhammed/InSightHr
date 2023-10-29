using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.APPCONTEXT;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.EntityFramework;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Concretes
{
    public class EmployeeDebitRepository : BaseRepository<EmployeeDebit>, IEmployeeDebitRepository
    {
        public EmployeeDebitRepository(AppDBContext context) : base(context)
        {
        }
    }
}
