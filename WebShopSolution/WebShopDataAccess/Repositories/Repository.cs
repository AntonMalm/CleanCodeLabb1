using Microsoft.EntityFrameworkCore;
using WebShopDataAccess.Repositories.Interfaces;

namespace WebShopDataAccess.Repositories
{
    public class Repository<T>(WebShopDbContext context) : IRepository<T>
        where T : class
    {
        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var foundEntity = await context.Set<T>().FindAsync(id);
            if(foundEntity == null)
                throw new Exception("Entity not found");

            return foundEntity;
        }

        public async Task AddAsync(T entity)
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            context.Set<T>().Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await context.Set<T>().FindAsync(id);
            if (entity != null)
            {
                context.Set<T>().Remove(entity);
                await context.SaveChangesAsync();
            }
        }
    }
}
