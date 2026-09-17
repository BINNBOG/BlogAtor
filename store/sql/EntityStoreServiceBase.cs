namespace BlogAtor.Store.Sql;

using BlogAtor.Framework;
using BlogAtor.Framework.Entity;
using BlogAtor.Framework.Store;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;

using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;

internal class EntityStoreServiceBase<ET, DBT> : ServiceBase, IEntityStore<ET> where ET : EntityBase
    where DBT : EntityBase, IDbEntity<ET>, new()
{
    private readonly BlogContext _context;
    protected BlogContext Context => _context;
    private readonly DbSet<DBT> _dbSet;
    protected DbSet<DBT> DbSet => _dbSet;

    private readonly IStoreObserver _observer;
    public EntityStoreServiceBase(BlogContext context, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _context = context;
        _dbSet = _context.GetDbSet<DBT>();

        _observer = GetService<IStoreObserver>();
    }
    private DBT ToData(ET entity)
    {
        var dbt = (DBT?)Activator.CreateInstance(typeof(DBT), [entity]);
        return dbt ?? throw new NotImplementedException();
    }
    public async Task<ET> AddEntityAsync(ET entity)
    {
        var dbEntity = await _dbSet.AddAsync(ToData(entity));
        await _context.SaveChangesAsync();

        var result = dbEntity.Entity.ToEntity();
        await _observer.NotifyChange([result], StoreChangeType.Added);
        return result;
    }
    public async Task<ICollection<ET>> AddEntityRangeAsync(ICollection<ET> entities)
    {
        var dbEntities = entities.Select(e => ToData(e)).ToList();
        await _dbSet.AddRangeAsync(dbEntities);
        await _context.SaveChangesAsync();

        var result = dbEntities.Select(db => db.ToEntity()).ToList();
        await _observer.NotifyChange(result, StoreChangeType.Added);
        return result;
    }
    public async Task<System.Boolean> DeleteEntityAsync(ET entity)
    {
        var toDelete = await _dbSet.SingleOrDefaultAsync(et => et.Id == entity.Id);
        if (toDelete == null)
        {
            return false;
        }
        _dbSet.Remove(toDelete);
        await _context.SaveChangesAsync();
        
        await _observer.NotifyChange([toDelete.ToEntity()], StoreChangeType.Deleted);
        return true;
    }
    public async Task<ICollection<ET>> GetAllAsync()
    {
        return await _dbSet.Select(dbet => dbet.ToEntity()).ToListAsync();
    }
    public async Task<ET?> GetByIdAsync(System.Int64 id)
    {
        return (await _dbSet.SingleOrDefaultAsync(et => et.Id == id))?.ToEntity();
    }

    private Expression<Func<DBT, System.Boolean>> CastPredicate(Expression<Func<ET, System.Boolean>> predicate)
    {
        var parameter = Expression.Parameter(typeof(DBT), "dbt");
        var body = Expression.Invoke(predicate, parameter);
        var dbPredicate = Expression.Lambda<Func<DBT, System.Boolean>>(body, parameter);
        return dbPredicate;
    }

    private Expression<Func<DBT, T>> CastOrderSelector<T>(Expression<Func<ET, T>> selector)
    {
        var parameter = Expression.Parameter(typeof(DBT), "dbt");
        var body = Expression.Invoke(selector, parameter);
        var dbPredicate = Expression.Lambda<Func<DBT, T>>(body, parameter);
        return dbPredicate;
    }

    public async Task<System.Int32> CountAsync(Expression<Func<ET, System.Boolean>>? predicate = null)
    {
        if (predicate == null)
        {
            return await _dbSet.CountAsync();
        }
        return await _dbSet.CountAsync(CastPredicate(predicate));
    }

    public async Task<ICollection<ET>> SelectAsync(System.Int32 skip, System.Int32 take,
        Expression<Func<ET, System.Boolean>>? where = null,
        Expression<Func<ET, System.Object>>? orderBy = null, System.Boolean orderDescending = false)
    {
        var selected = _dbSet.AsQueryable();
        if (where != null)
        {
            selected = selected.Where(CastPredicate(where));
        }
        if (orderBy != null)
        {
            var orderSelector = CastOrderSelector(orderBy);
            selected = orderDescending
                ? selected.OrderByDescending(orderSelector)
                : selected.OrderBy(orderSelector);
        }

        var results = await selected.Skip(skip).Take(take).ToListAsync();
        return results.Select(s => s.ToEntity()).ToList();
    }
}