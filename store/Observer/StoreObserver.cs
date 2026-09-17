namespace BlogAtor.Store.Observer;

using System.Threading.Tasks.Dataflow;

using BlogAtor.Framework;
using BlogAtor.Framework.Entity;
using BlogAtor.Store.Abstrations;

using Microsoft.Extensions.Logging;

public class StoreObserver : ModuleBase, IStoreObserver
{
    private readonly BufferBlock<(EntityBase, StoreChangeType)> _bufferBlock;
    private Dictionary<(Type, StoreChangeType), ICollection<Func<ICollection<EntityBase>, Task>>> _callbacks;
    public StoreObserver(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _bufferBlock = new BufferBlock<(EntityBase, StoreChangeType)>();
        _callbacks = new Dictionary<(Type, StoreChangeType), ICollection<Func<ICollection<EntityBase>, Task>>>();
        AddPeriodicJob(SendNotifications, TimeSpan.FromMilliseconds(100));
    }
    public void Subscribe<ET>(Func<ICollection<EntityBase>, Task> asyncCallback, StoreChangeType changeType) where ET : EntityBase
    {
        var key = (typeof(ET), changeType);
        if (!_callbacks.ContainsKey(key))
        {
            _callbacks.Add(key, new List<Func<ICollection<EntityBase>, Task>>());
        }
        _callbacks[key].Add(asyncCallback);
    }
    private System.Boolean IsInteresting(Type entityType, StoreChangeType changeType)
        => _callbacks.ContainsKey((entityType, changeType));
    public Task NotifyChange<ET>(ICollection<ET> changed, StoreChangeType changeType) where ET : EntityBase
    {
        if (!IsInteresting(typeof(ET), changeType))
        {
            return Task.CompletedTask;
        }
        foreach (var change in changed)
        {
            _bufferBlock.Post((change, changeType));
        }
        return Task.CompletedTask;
    }
    private async Task SendNotifications()
    {
        var batches = new Dictionary<(Type, StoreChangeType), ICollection<EntityBase>>();

        while (_bufferBlock.TryReceive(out var pair))
        {
            var (entity, change) = pair;
            var key = (entity.GetType(), change);

            if (!batches.ContainsKey(key))
            {
                batches.Add(key, new List<EntityBase>());
            }
            batches[key].Add(entity);
        }

        if (batches.Count == 0)
        {
            return;
        }

        var actionBlock = new ActionBlock<(Func<ICollection<EntityBase>, Task>, ICollection<EntityBase>)>(NotificationSender);
        foreach (var (key, batch) in batches)
        {
            var callbacks = _callbacks[key];
            foreach (var callback in callbacks)
            {
                actionBlock.Post((callback, batch));
            }
        }
        actionBlock.Complete();
        await actionBlock.Completion;
    }
    private async Task NotificationSender((Func<ICollection<EntityBase>, Task>, ICollection<EntityBase>) pair)
    {
        var (callback, batch) = pair;
        await callback(batch);
    }
}
