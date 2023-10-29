using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using MVC_ONION_PROJECT.DOMAIN.CORE.Base;
using MVC_ONION_PROJECT.DOMAIN.ENTITIES;
using MVC_ONION_PROJECT.DOMAIN.ENUMS;
using MVC_ONION_PROJECT.INFRASTRUCTURE.Configurations;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace MVC_ONION_PROJECT.INFRASTRUCTURE.APPCONTEXT
{
    public class AppDBContext : IdentityDbContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppDBContext(DbContextOptions<AppDBContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {

        }


        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Organization> Organization { get; set; }
        public virtual DbSet<EmployeeSalary> EmployeeSalary { get; set; }
        public virtual DbSet<Department> Department { get; set; }
        public virtual DbSet<Package> Package { get; set; }
        public virtual DbSet<EmployeeDebit> EmployeeDebit { get; set; }
        public virtual DbSet<TimeOff> TimeOff { get; set; }
        public virtual DbSet<OrgAsset> OrgAsset { get; set; }
        public virtual DbSet<AssetCategory> AssetCategory { get; set; }
        public virtual DbSet<TimeOffType> TimeOffType { get; set; }
        public virtual DbSet<AdvancePayment> AdvancePayments { get; set; }
        public virtual DbSet<Reimbursement> Reimbursement { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(IEntityConfiguration).Assembly);
            base.OnModelCreating(builder);
        }

       public override int SaveChanges()
        {
            SetBaseProperties();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetBaseProperties();
            return base.SaveChangesAsync(cancellationToken);
        }


        private void SetBaseProperties()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            var userId = _httpContextAccessor?.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "user bulunamadı";

            foreach (var entry in entries)
            {
                SetIfAdded(entry, userId);
                SetIfModified(entry, userId);
                SetIfDeleted(entry, userId);
            }
        }

        private void SetIfDeleted(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State != EntityState.Deleted)
            {
                return;
            }

            if (entry.Entity is not AuditableEntity entity)
            {
                return;
            }

            entry.State = EntityState.Modified;
            entity.DeletedDate = DateTime.Now;
            entity.Status = Status.Deleted;
            entity.DeletedBy = userId;
        }

        private void SetIfModified(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Modified)
            {
                entry.Entity.Status = Status.Updated;
                entry.Entity.UpdatedBy = userId;
                entry.Entity.UpdatedDate = DateTime.Now;
            }
        }

        private void SetIfAdded(EntityEntry<BaseEntity> entry, string userId)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.Status = Status.Created;
                entry.Entity.CreatedDate = DateTime.Now;
                entry.Entity.CreatedBy = userId;
            }
        }

    }
}