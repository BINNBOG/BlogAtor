namespace BlogAtor.Collector.Abstractions;

using System.Threading;
using System.Threading.Tasks.Dataflow;

using BlogAtor.Core.Entity;
using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

using Microsoft.Extensions.Logging;

public class CollectorManagerService : ModuleBase, IDataSourceCollectorManager
{
    private IDictionary<System.String, IDataSourceCollector> _collectors;
    private readonly BufferBlock<DataItem> _saveItemsBlock;
    private readonly IDataItemStore _store;
    private readonly BufferBlock<DataItem> _saveBlock;
    public CollectorManagerService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _collectors = GetServices<IDataSourceCollector>().ToDictionary(c => c.Type);
        _saveItemsBlock = new BufferBlock<DataItem>(new DataflowBlockOptions());
        _store = GetService<IDataItemStore>();

        _saveBlock = new BufferBlock<DataItem>();
        AddPeriodicJob(SaveDataItemsAsync, TimeSpan.FromSeconds(5));
    }
    public ICollection<System.String> CollectorTypes => _collectors.Keys.ToList();
    public async Task<System.Boolean> ValidateCollectorConfiguration(System.String type, System.Int64 configId)
    {
        IDataSourceCollector? collector;
        if (!_collectors.TryGetValue(type, out collector) || collector == null)
        {
            return false;
        }
        return await collector.ValidateParameter(configId);
    }
    protected override async Task OnStopAsync(CancellationToken cancellationToken)
    {
        await base.OnStopAsync(cancellationToken);
        _saveItemsBlock.Complete();
        await _saveItemsBlock.Completion;
    }

    public Task SubmitDataItemAsync(DataItem dataItem)
    {
        _saveBlock.Post(dataItem);
        return Task.CompletedTask;
    }

    private readonly System.Int32 cMaxSaveBatchSize = 1024;
    private async Task SaveDataItemsAsync()
    {
        DataItem? recieved;
        List<DataItem> toSave = new List<DataItem>();

        while (_saveBlock.TryReceive(out recieved) && toSave.Count < cMaxSaveBatchSize)
        {
            toSave.Add(recieved);
        }

        if (toSave.Count == 0)
        {
            return;
        }

        // saving to database
        Logger.LogInformation($"Trying to save {toSave.Count()} data items to store");
        try
        {
            var saved = await _store.SaveNewDataItemsAsync(toSave);
            Logger.LogInformation($"Saved {saved.Count} data items to store");
        }
        catch (Exception ex)
        {
            Logger.LogError($"Error saving items to DB: {ex}");
        }
    }
}