namespace BlogAtor.Core.Config;

public class ConfigBase<T> : IConfig where T : class
{
    public ConfigBase()
    {
    }
    public ConfigBase(System.Int64 id, T config)
    {
        Id = id;
        Configuration = config;
    }
    public System.Int64 Id { get; set; }
    public T Configuration { get; set; }
}