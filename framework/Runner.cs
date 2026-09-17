namespace BlogAtor.Framework;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Runner
{
    private readonly HostApplicationBuilder _builder;
    private IHost? _host;
    public Runner()
    {
        _builder = Host.CreateApplicationBuilder();
    }
    public IServiceCollection Services => _builder.Services;
    public void AddService<SI, ST>() where ST : ServiceBase, SI where SI : class
    {
        _builder.Services.AddSingleton<SI, ST>();
    }
    public void AddTransient<SI, ST>() where ST : ServiceBase, SI where SI : class
    {
        _builder.Services.AddTransient<SI, ST>();
    }
    public void AddModule<MT>() where MT : ModuleBase
    {
        _builder.Services.AddSingleton<MT>();
        _builder.Services.AddHostedService<ModuleHost<MT>>();
    }
    public void AddModule<MI, MT>() where MT : ModuleBase, MI where MI : class, IModule
    {
        _builder.Services.AddSingleton<MI, MT>();
        _builder.Services.AddHostedService<ModuleHost<MI>>();
    }
    public void Build()
    {
        _host = _builder.Build();
    }
    public async Task RunAsync()
    {
        if (_host == null)
        {
            throw new NotImplementedException($"Host was not built");
        }
        await _host.RunAsync();
    }
}