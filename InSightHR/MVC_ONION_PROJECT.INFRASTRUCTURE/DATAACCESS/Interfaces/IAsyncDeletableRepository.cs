using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces
{
    public interface IAsyncDeletableRepository<TEntity>: IAsyncRepository where TEntity : BaseEntity
    {
        //Task DeleteAsync(string id);

        //Buradan servise gideceği için id yerine TEntity titpinde değer alıyoruz.
        Task DeleteAsync(TEntity entity);
        Task DeleteRangeAsync(IEnumerable<TEntity> entities);
    }
}
