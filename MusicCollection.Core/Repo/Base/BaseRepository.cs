

using Microsoft.EntityFrameworkCore;
using MusicCollection.Core.EfStructures;
using MusicCollection.Models.Entities.Base;

namespace MusicCollection.Core.Repo.Base;

public class BaseRepository<T> : IRepository<T> where T : BaseEntity, new()
{
    protected readonly ApplicationDbContext Context;
    protected readonly DbSet<T> Table;


    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
        Table = context.Set<T>();
    }

    public virtual async Task<T?> GetByIdAsync(int id) => await Table.FindAsync(id);
    public virtual async Task<List<T>> GetAllAsync() => await Table.ToListAsync();
    public virtual async Task AddAsync(T entity) => await Table.AddAsync(entity);
    public virtual async Task AddRangeAsync(IEnumerable<T> entities) => await Table.AddRangeAsync(entities);
    public virtual void Update(T entity) => Table.Update(entity);
    public virtual void UpdateRange(IEnumerable<T> entities) => Table.UpdateRange(entities);
    public virtual void Delete(T entity) => Table.Remove(entity);
    public virtual void DeleteRange(IEnumerable<T> entities) => Table.RemoveRange(entities);
    public virtual async Task SaveChangesAsync() => await Context.SaveChangesAsync();
}
