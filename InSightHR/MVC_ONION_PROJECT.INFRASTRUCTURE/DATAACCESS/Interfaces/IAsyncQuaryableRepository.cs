using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces
{
    public interface IAsyncQuaryableRepository<TEntity>: IAsyncRepository where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool tracking=true);
        Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression,bool tracking=true);
    }
}
