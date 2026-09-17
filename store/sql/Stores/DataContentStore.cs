namespace BlogAtor.Store.Sql;

using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;

internal class DataContentStore : EntityStoreServiceBase<DataContent, DbDataContent>, IDataContentStore
{
    public DataContentStore(BlogContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }
}