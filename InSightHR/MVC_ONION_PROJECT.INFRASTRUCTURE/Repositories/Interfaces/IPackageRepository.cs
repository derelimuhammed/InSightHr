using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.Repositories.Interfaces
{
    public interface IPackageRepository: IAsyncRepository, IAsyncFindableRepository<Package>, IAsyncInsertableRepository<Package>, IAsyncOrderableRepository<Package>, IAsyncUpdatableRepository<Package>, IAsyncQuaryableRepository<Package>, IAsyncDeletableRepository<Package>
    {
    }
}
