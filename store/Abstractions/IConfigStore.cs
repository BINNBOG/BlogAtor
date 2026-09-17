namespace BlogAtor.Store.Abstrations;

using BlogAtor.Core.Config;

public interface IConfigStore
{
    public Task<IConfig?> GetConfigById(System.Int64 id);
    public Task<IDictionary<System.Int64, IConfig?>> GetConfigsByIds(ICollection<System.Int64> ids);
    public Task<System.Boolean> DeleteConfigById(System.Int64 id);
    public Task<ICollection<ConfigBase<T>>> GetConfigurationsOfType<T>() where T : class;
    public Task<ConfigBase<T>?> GetConfiguration<T>(System.Int64 id) where T : class;
    public Task<System.Int64> SaveConfiguration<T>(ConfigBase<T> configuration) where T : class;
}