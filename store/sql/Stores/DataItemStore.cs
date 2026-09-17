namespace BlogAtor.Store.Sql;

using BlogAtor.Core.Entity;
using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;

internal class DataItemStore : EntityStoreServiceBase<DataItem, DbDataItem>, IDataItemStore
{
    public DataItemStore(BlogContext context, IServiceProvider serviceProvider) : base(context, serviceProvider)
    {
    }

    public async Task<ICollection<DataItem>> SaveNewDataItemsAsync(ICollection<DataItem> dataItems)
    {
        var uids = dataItems.Select(di => di.Uid).ToHashSet();
        var existing = Context.DataItems.Where(di => uids.Contains(di.Uid));
        var existingUids = existing.Select(di => di.Uid).ToHashSet();
        var newDataItems = dataItems.Where(di => !existingUids.Contains(di.Uid)).ToList();

        var result = await AddEntityRangeAsync(newDataItems);
        return result;
    }
}