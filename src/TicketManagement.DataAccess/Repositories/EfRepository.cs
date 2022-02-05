using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TicketManagement.DataAccess.Interfaces;

namespace TicketManagement.DataAccess.Repositories
{
    /// <summary>
    /// Entity framework repository.
    /// </summary>
    /// <typeparam name="T">Entity.</typeparam>
   internal class EfRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EfRepository{T}"/> class.
        /// </summary>
        /// <param name="dbContext">Object of TicketManagementContext.</param>
        public EfRepository(TicketManagementContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <summary>
        /// Gets or privatly sets object of DbContext.
        /// </summary>
        protected TicketManagementContext DbContext { get; private set; }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await DbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(e => e.Id == id);
        }

        public virtual async Task<IQueryable<T>> GetAllAsync()
        {
            return await Task.Run(() => DbContext.Set<T>().AsNoTracking());
        }

        public virtual async Task<T> CreateAsync(T obj)
        {
            await DbContext.Set<T>().AddAsync(obj);
            await DbContext.SaveChangesAsync();
            return obj;
        }

        public virtual async Task<T> UpdateAsync(T obj)
        {
            EntityEntry<T> temp = DbContext.Entry(obj);
            temp.State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            return temp.Entity;
        }

        public virtual async Task<T> DeleteAsync(T obj)
        {
            T oldEntity = DbContext.Set<T>().Remove(obj).Entity;
            await DbContext.SaveChangesAsync();
            return oldEntity;
        }
    }
}
