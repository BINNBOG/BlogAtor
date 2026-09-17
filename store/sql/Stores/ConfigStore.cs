namespace BlogAtor.Store.Sql;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using BlogAtor.Store.Abstrations;
using BlogAtor.Store.Sql.Data;
using BlogAtor.Core.Config;
using BlogAtor.Framework;

using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

internal class ConfigStore : ServiceBase, IConfigStore
{
    private readonly BlogContext _context;
    public ConfigStore(BlogContext context, IServiceProvider serviceProvider) :
        base(serviceProvider)
    {
        _context = context;
    }
    private System.String SerializeConfig<T>(T config) where T : class
    {
        return JsonSerializer.Serialize<T>(config);
    }
    private T DeserializeConfig<T>(System.String configJson) where T : class
    {
        return JsonSerializer.Deserialize<T>(configJson) ?? throw new NotImplementedException();
    }
    private IConfig? ResolveDbConfigType(DbConfig dbConfig)
    {
        var assembly = AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(a => a.FullName == dbConfig.ConfigAssembly);
        if (assembly == null)
        {
            return null;
        }

        var configType = assembly.GetType(dbConfig.ConfigType);
        if (configType == null)
        {
            return null;
        }

        var config = JsonSerializer.Deserialize(dbConfig.JsonConfig, configType);

        var configBaseType = typeof(ConfigBase<>).MakeGenericType([configType]);
        var configBase = Activator.CreateInstance(configBaseType, [dbConfig.Id, config]);
        
        return configBase as IConfig;
    }
    public async Task<IConfig?> GetConfigById(System.Int64 id)
    {
        var dbConfig = await _context.Configs.SingleOrDefaultAsync(c => c.Id == id);
        if (dbConfig == null)
        {
            return null;
        }
        return ResolveDbConfigType(dbConfig);
    }
    public async Task<IDictionary<System.Int64, IConfig?>> GetConfigsByIds(ICollection<System.Int64> ids)
    {
        var dbConfigs = await _context.Configs.Where(c => ids.Contains(c.Id)).ToListAsync();
        return dbConfigs.Select(dbc => ResolveDbConfigType(dbc)).ToDictionary(c => c.Id);
    }

    public async Task<ConfigBase<T>?> GetConfiguration<T>(System.Int64 id) where T : class
    {
        var dbConfig = await _context.Configs.SingleOrDefaultAsync(c => c.Id == id && c.ConfigType == typeof(T).FullName);
        if (dbConfig == null)
        {
            return null;
        }
        return new ConfigBase<T>()
        {
            Id = id,
            Configuration = DeserializeConfig<T>(dbConfig.JsonConfig)
        };
    }

    public async Task<ICollection<ConfigBase<T>>> GetConfigurationsOfType<T>() where T : class
    {
        var configType = typeof(T).FullName;

        var dbConfigs = await _context.Configs.Where(c => c.ConfigType == configType).ToListAsync();
        var result = dbConfigs.Select(dbc => new ConfigBase<T>()
        {
            Id = dbc.Id,
            Configuration = DeserializeConfig<T>(dbc.JsonConfig)
        }).ToList();
        return result;
    }

    public async Task<System.Int64> SaveConfiguration<T>(ConfigBase<T> configuration) where T : class
    {
        var serializedConfig = SerializeConfig(configuration.Configuration);
        var configData = new DbConfig()
        {
            ConfigAssembly = typeof(T).Assembly.FullName!,
            ConfigType = typeof(T).FullName!,
            JsonConfig = serializedConfig
        };

        var result = await _context.Configs.AddAsync(configData);
        await _context.SaveChangesAsync();

        return result.Entity.Id;
    }

    public async Task<System.Boolean> DeleteConfigById(System.Int64 id)
    {
        var config = await _context.Configs.SingleOrDefaultAsync(c => c.Id == id);
        if (config == null)
        {
            return false;
        }
        _context.Configs.Remove(config);
        await _context.SaveChangesAsync();
        return true;
    }
}