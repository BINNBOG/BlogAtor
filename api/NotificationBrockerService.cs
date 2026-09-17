namespace BlogAtor.API;

using BlogAtor.API.Hub;
using BlogAtor.Core.Entity;
using BlogAtor.Framework;
using BlogAtor.Framework.Entity;
using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.SignalR;

internal class NotificationBrockerService : ServiceBase
{
    private readonly IStoreObserver _observer;

    private readonly IHubContext<EntityChangeHub> _dataSourceChangeHub;

    public NotificationBrockerService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _observer = GetService<IStoreObserver>();

        _dataSourceChangeHub = GetService<IHubContext<EntityChangeHub>>();

        _observer.Subscribe<DataSource>(OnDataSourceAdd, StoreChangeType.Added);
        _observer.Subscribe<DataSource>(OnDataSourceDelete, StoreChangeType.Deleted);

        _observer.Subscribe<SourceCollector>(OnSourceCollectorAdd, StoreChangeType.Added);
        _observer.Subscribe<SourceCollector>(OnSourceCollectorDelete, StoreChangeType.Deleted);

        _observer.Subscribe<DataItem>(OnDataItemAdd, StoreChangeType.Added);
        _observer.Subscribe<DataItem>(OnDataItemDelete, StoreChangeType.Deleted);
    }
    private readonly System.String cOnDataSourceChange = "OnDataSourceChange";
    private readonly System.String cOnSourceCollectorChange = "OnSourceCollectorChange";
    private readonly System.String cOnDataItemChange = "OnDataItemChange";

    private async Task OnDataSourceAdd(ICollection<EntityBase> added)
        => await OnEntityChange<DataSource>(cOnDataSourceChange, added, StoreChangeType.Added);
    private async Task OnDataSourceDelete(ICollection<EntityBase> deleted)
        => await OnEntityChange<DataSource>(cOnDataSourceChange, deleted, StoreChangeType.Deleted);

    private async Task OnSourceCollectorAdd(ICollection<EntityBase> added)
        => await OnEntityChange<SourceCollector>(cOnSourceCollectorChange, added, StoreChangeType.Added);
    private async Task OnSourceCollectorDelete(ICollection<EntityBase> deleted)
        => await OnEntityChange<SourceCollector>(cOnSourceCollectorChange, deleted, StoreChangeType.Deleted);

    private async Task OnDataItemAdd(ICollection<EntityBase> added)
        => await OnEntityChange<DataItem>(cOnDataItemChange, added, StoreChangeType.Added);
    private async Task OnDataItemDelete(ICollection<EntityBase> deleted)
        => await OnEntityChange<DataItem>(cOnDataItemChange, deleted, StoreChangeType.Deleted);

    private async Task OnEntityChange<ET>(System.String changeEndpoint, ICollection<EntityBase> changes, StoreChangeType changeType) where ET : EntityBase
    {
        var notification = new ChangeNotification<ET>(changes.Cast<ET>().ToList(), changeType);
        await _dataSourceChangeHub.Clients.All.SendAsync(changeEndpoint, notification);
    }
}