using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.DATAACCESS.EntityFramework
{
    public abstract class BaseRepository<TEntity> : IRepository, IAsyncRepository, IAsyncFindableRepository<TEntity>,
        IAsyncInsertableRepository<TEntity>, IAsyncQuaryableRepository<TEntity>, IAsyncOrderableRepository<TEntity>, IAsyncDeletableRepository<TEntity>, ITransactionRepository,
        IAsyncUpdatableRepository<TEntity> where TEntity : BaseEntity
    {

        private readonly DbContext _context;  //vt bağlantısı
        private readonly DbSet<TEntity> _table; //Hangi entity ile ilgili işlem yapılıyorsa

        public BaseRepository(DbContext context)
        {
            _context= context;
            _table= _context.Set<TEntity>();
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            var entry=await _table.AddAsync(entity);  //AddAsync, DBContext ten gelen default bir metottur, bizim yaptığımız birşey değil
            return entry.Entity;
        }

        public Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            return _table.AddRangeAsync(entities);
        }

        public Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? expression = null)
        {
            return expression is null ? GetAllActives().AnyAsync() : GetAllActives().AnyAsync(expression);
        }

        public Task DeleteAsync(TEntity entity)
        {
            return Task.FromResult(_table.Remove(entity));  //DeleteAsync metodunun asenkron bir dönüşü yoktur.
            //task olarak döndürmek için Task.FromResult() kalıbını kullanırız
            //Veri tabanında asenkron silme işlemi yok 
            //servisimizin komple asenkron çalışması için bu işlemi yapıyoruz.
        }

        public Task DeleteRangeAsync(IEnumerable<TEntity> entities)
        {
            _table.RemoveRange(entities);
            return _context.SaveChangesAsync(); //Task dönüşünde olması için SaveChangesAsync() metodunu kullanıyoruz
            //Silme işlemi save change işlemi olduğu için SaveChangesAsync() diyerek 2.kez yazmış oluyoruz
            //Ancak 2.kez yazmak sorun oluşturmuyor
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool tracking = true)
        {
            return await GetAllActives(tracking).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, bool tracking = true)
        {
            return await GetAllActives(tracking).Where(expression).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, double>> orderby, bool orderDesc = false, bool tracking = true)
        {
            var values = GetAllActives(tracking);
            return orderDesc ? await values.OrderByDescending(orderby).ToListAsync() : await values.OrderBy(orderby).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> expression, Expression<Func<TEntity, double>> orderby, bool orderDesc = false, bool tracking = true)
        {
            var values = GetAllActives(tracking).Where(expression);
            return orderDesc ? await values.OrderByDescending(orderby).ToListAsync() : await values.OrderBy(orderby).ToListAsync();
        }

        public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>>? expression, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(expression);
        }

        public async Task<TEntity?> GetByIdAsync(Guid? id, bool tracking = true)
        {
            return await GetAllActives(tracking).FirstOrDefaultAsync(x=>x.Id==id);
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            var entry= await Task.FromResult(_table.Update(entity));
            return entry.Entity;
        }

        protected IQueryable<TEntity> GetAllActives(bool tracking = true)
        {
            var values = _table.Where(x => x.Status != Status.Deleted);
            return tracking ? values : values.AsNoTracking(); //True değilse trackingden çık
        }
        public Task<IExecutionStrategy> CreateExecutionStrategy()
        {
            return Task.FromResult(_context.Database.CreateExecutionStrategy());
        }
        public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            return _context.Database.BeginTransactionAsync(cancellationToken);
        }
    }
}
