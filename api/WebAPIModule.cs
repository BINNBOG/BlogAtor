namespace BlogAtor.API;

using BlogAtor.API.Hub;
using BlogAtor.Collector.Abstractions;
using BlogAtor.Framework;
using BlogAtor.Security.Authentication;
using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

internal partial class WebAPIModule : ModuleBase
{
    private readonly WebApplicationBuilder _builder;
    private WebApplication? _app;
    public WebAPIModule(IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _builder = WebApplication.CreateBuilder();
        
        AddJwtAuthentication();

        _builder.Services.AddControllers()
            .AddApplicationPart(typeof(WebAPIModule).Assembly);
        _builder.Services.AddSignalR();
        _builder.Services.AddOpenApi();
        _builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.SetIsOriginAllowed((host) => true)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        PopulateApiServices();
        _app = _builder.Build();
        
        _app.MapOpenApi(); // TODO: add debug switch
        _app.UseHttpsRedirection();
        _app.UseCors();

        _app.UseAuthentication();
        _app.UseAuthorization();
        
        _app.MapControllers();
        
        RegisterHubs();
    }
    private void RegisterHubs()
    {
        if (_app == null)
        {
            throw new NotImplementedException();
        }
        var webSocketOptions = new WebSocketOptions()
        {
            KeepAliveInterval = TimeSpan.FromMinutes(2)
        };
        webSocketOptions.AllowedOrigins.Add("*");

        _app.UseWebSockets(webSocketOptions);
        _app.MapHub<EntityChangeHub>("/changes");
    }
    private void PassthroughService<SI>() where SI : class
    {
        _builder.Services.AddSingleton<SI>(GetService<SI>());
    }
    private void PassthroughScoped<SI>() where SI : class
    {
        _builder.Services.AddScoped<SI>(_ => GetService<SI>());
    }
    private void PopulateApiServices()
    {
        // add store services
        PassthroughScoped<IConfigStore>();

        PassthroughScoped<IUserStore>();

        PassthroughScoped<IDataSourceStore>();
        PassthroughScoped<ISourceCollectorStore>();
        PassthroughScoped<IDataItemStore>();

        PassthroughService<IStoreObserver>();

        // add auth services
        PassthroughService<IAuthenticationService>();

        // add collector service
        PassthroughService<IHttpClientProvider>();
        PassthroughService<IDataSourceCollectorManager>();
        
        // add logger
        _builder.Logging.ClearProviders();
        PassthroughService<ILoggerFactory>();

        // add web services
        _builder.Services.AddSingleton<NotificationBrockerService>();
    }
    private void WarmUpServices()
    {
        // notification broker service should be triggered in order to register subscription to StoreObserver
        _app?.Services.GetService<NotificationBrockerService>();
    }
    protected override async Task OnStartAsync(CancellationToken cancellationToken)
    {
        await base.OnStartAsync(cancellationToken);
        if (_app != null)
        {
            await _app.StartAsync(cancellationToken);
            WarmUpServices();
        }
    }

    protected override async Task OnStopAsync(CancellationToken cancellationToken)
    {
        await base.OnStopAsync(cancellationToken);
        if (_app != null)
        {
            await _app.StopAsync(cancellationToken);
        }
    }
}
