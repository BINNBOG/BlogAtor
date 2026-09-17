namespace BlogAtor.Store.Abstrations;

using BlogAtor.Framework;
using BlogAtor.Framework.Entity;

public interface IStoreObserver : IModule
{
    public void Subscribe<ET>(Func<ICollection<EntityBase>, Task> asyncCallback, StoreChangeType changeType) where ET : EntityBase;
    public Task NotifyChange<ET>(ICollection<ET> changed, StoreChangeType changeType) where ET : EntityBase;
}