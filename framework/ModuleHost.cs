namespace BlogAtor.Framework;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

internal class ModuleHost<M> : IHostedService where M : IModule
{
    private readonly M _hostedModule;

    public ModuleHost(IServiceProvider serviceProvider)
    {
        _hostedModule = serviceProvider.GetService<M>() ?? throw new NotImplementedException();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _hostedModule.StartAsync(cancellationToken);
    }
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _hostedModule.StopAsync(cancellationToken);
    }
}