using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces
{
    public interface IAsyncFindableRepository<TEntity>: IAsyncRepository where TEntity : BaseEntity
    {
        Task<bool> AnyAsync(Expression<Func<TEntity,bool>>? expression=null);
        Task<TEntity?> GetByIdAsync(Guid? id, bool tracking=true);
        Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? expression, bool tracking = true);
    }
}
