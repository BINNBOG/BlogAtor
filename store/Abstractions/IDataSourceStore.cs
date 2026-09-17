namespace BlogAtor.Store.Abstrations;

using BlogAtor.Core.Entity;
using BlogAtor.Framework.Store;

public interface IDataSourceStore : IEntityStore<DataSource>
{
    public Task<ICollection<SourceCollector>> GetDataSourceCollectors(System.Int64 dataSourceId);
}