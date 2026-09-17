namespace BlogAtor;

using BlogAtor.Core;
using BlogAtor.Core.Config;
using BlogAtor.Framework;

using System.Text.Json;

public class StartupConfigProviderService : ServiceBase, IStartupConfigProvider
{
    private readonly System.String ConfigDir = "/etc/blogator";

    private readonly StartupConfig _startupConfig;

    public StartupConfig StartupConfig => _startupConfig;

    private T ReadConfig<T>() where T : class
    {
        var configPath = Path.Combine(ConfigDir, $"{typeof(T).Name}.json");

        if (!File.Exists(configPath))
        {
            throw new NotImplementedException($"Invalid config path <type={typeof(T).Name} path={configPath}>");
        }

        var rawData = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<T>(rawData) ?? throw new NotImplementedException($"Failed parsing config");
    }

    public StartupConfigProviderService(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _startupConfig = ReadConfig<StartupConfig>();
    }
}