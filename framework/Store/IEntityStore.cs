namespace BlogAtor.Framework.Store;

using System.Linq.Expressions;

using BlogAtor.Framework.Entity;

public interface IEntityStore<ET> where ET : EntityBase
{
    public Task<ICollection<ET>> GetAllAsync();
    public Task<ET?> GetByIdAsync(System.Int64 id);
    public Task<ET> AddEntityAsync(ET entity);
    public Task<System.Boolean> DeleteEntityAsync(ET entity);

    public Task<System.Int32> CountAsync(Expression<Func<ET, System.Boolean>>? predicate = null);
    public Task<ICollection<ET>> SelectAsync(System.Int32 skip, System.Int32 take,
        Expression<Func<ET, System.Boolean>>? where = null,
        Expression<Func<ET, System.Object>>? orderBy = null, System.Boolean orderDescending = false);
}