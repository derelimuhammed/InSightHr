using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces
{
    public interface IOrganizationRepository : IAsyncRepository, IAsyncFindableRepository<Organization>, IAsyncInsertableRepository<Organization>, IAsyncOrderableRepository<Organization>, IAsyncUpdatableRepository<Organization>, IAsyncQuaryableRepository<Organization>,IAsyncDeletableRepository<Organization>
    {
    }
}
