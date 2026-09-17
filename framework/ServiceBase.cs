namespace BlogAtor.Framework;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

public abstract class ServiceBase
{
    private readonly IServiceProvider _serviceProvider;
    protected readonly ILogger _logger;
    public ServiceBase(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _logger = GetLogger();
    }
    protected ILogger GetLogger(Type? type = null)
    {
        if (type == null)
        {
            type = this.GetType();
        }
        var loggerType = typeof(ILogger<>).MakeGenericType([type]);
        var logger = _serviceProvider.GetService(loggerType) as ILogger;
        return logger ??
            throw new NotImplementedException("Invalid logger type");
    }
    protected ILogger Logger => _logger;
    protected IS GetService<IS>() where IS : class
    {
        return _serviceProvider.GetService<IS>()
            ?? throw new NotImplementedException("Service not found");
    }
    protected ICollection<IS> GetServices<IS>() where IS : class
    {
        return _serviceProvider.GetServices<IS>().ToList();
    }
}