namespace BlogAtor.Store.Sql;

using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;

internal class SourceCollectorStore : EntityStoreServiceBase<SourceCollector, DbSourceCollector>, ISourceCollectorStore
{
    public SourceCollectorStore(BlogContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}