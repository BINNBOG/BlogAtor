namespace BlogAtor.Store.Abstrations;

using BlogAtor.Core.Entity;
using BlogAtor.Framework.Store;

public interface IDataItemStore : IEntityStore<DataItem>
{
    public Task<ICollection<DataItem>> SaveNewDataItemsAsync(ICollection<DataItem> dataItems);
}