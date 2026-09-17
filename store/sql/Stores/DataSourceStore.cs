namespace BlogAtor.Store.Sql;

using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;
using Microsoft.EntityFrameworkCore;

internal class DataSourceStore : EntityStoreServiceBase<DataSource, DbDataSource>, IDataSourceStore
{
    public DataSourceStore(BlogContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }

    public async Task<ICollection<SourceCollector>> GetDataSourceCollectors(System.Int64 dataSourceId)
    {
        return await Context.SourceCollectors.Where(sc => sc.DataSourceId == dataSourceId)
                        .Select(sc => sc.ToEntity())
                        .ToListAsync();
    }
}

