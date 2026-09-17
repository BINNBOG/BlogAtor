namespace BlogAtor.Collector.Abstractions;

using BlogAtor.Core.Entity;
using BlogAtor.Framework;

public interface IDataSourceCollectorManager : IModule
{
    public ICollection<System.String> CollectorTypes { get; }
    public Task<System.Boolean> ValidateCollectorConfiguration(System.String type, System.Int64 configId);
    public Task SubmitDataItemAsync(DataItem dataItem);
}