using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces
{
    public interface IAsyncOrderableRepository<TEntity>: IAsyncRepository where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, double>> orderby, bool orderDesc = false, bool tracking = true);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, double>> orderby, bool orderDesc=false, bool tracking = true);
    }
}
