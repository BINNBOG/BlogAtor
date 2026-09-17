namespace BlogAtor.Collector.Abstractions;

using BlogAtor.Framework;

public interface IDataSourceCollector : IModule
{
    public System.String Type { get; }
    public Task<System.Boolean> ValidateParameter(System.Int64 configId);
}