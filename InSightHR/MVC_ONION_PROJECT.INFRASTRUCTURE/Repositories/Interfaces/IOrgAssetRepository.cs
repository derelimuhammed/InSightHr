using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces
{
    public interface IOrgAssetRepository : IAsyncRepository, IAsyncFindableRepository<OrgAsset>, IAsyncInsertableRepository<OrgAsset>, IAsyncOrderableRepository<OrgAsset>, IAsyncUpdatableRepository<OrgAsset>, IAsyncQuaryableRepository<OrgAsset>, IAsyncDeletableRepository<OrgAsset>
    {
    }
}
