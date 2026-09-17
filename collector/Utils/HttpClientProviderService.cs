namespace BlogAtor.Collector.Utils;

using System.Net;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Collector.Configuration;
using BlogAtor.Framework;
using BlogAtor.Store.Abstrations;

public class HttpClientProviderService : ServiceBase, IHttpClientProvider
{
    private static readonly System.String[] cValidationDomains = [
        "google.com",
        "x.com"
    ];
    private readonly IConfigStore _store;
    public HttpClientProviderService(IConfigStore store, IServiceProvider serviceProvider) : base(serviceProvider)
    {
        _store = store;
    }
    public async Task<HttpClient?> GetProxyClient(System.Int64 proxyId)
    {
        var config = await _store.GetConfiguration<HttpProxyConfiguration>(proxyId);
        if (config == null)
        {
            return null;
        }
        return MakeHttpClient(config.Configuration);
    }
    public async Task<ICollection<System.Int64>> ListWebProxyIds()
    {
        var result = await _store.GetConfigurationsOfType<HttpProxyConfiguration>();
        return result.Select(r => r.Id).ToList();
    }
    public async Task<System.Boolean> ValidateHttpProxy(HttpProxyConfiguration configuration)
    {
        if (!Uri.TryCreate(configuration.Url, UriKind.Absolute, out _))
        {
            return false;
        }

        var client = MakeHttpClient(configuration);
        client.Timeout = TimeSpan.FromSeconds(5);

        foreach (var domain in cValidationDomains)
        {
            try
            {
                var resp = await client.GetAsync($"https://{domain}");
                if (resp.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            }
            catch (Exception ex)
            {
                // TODO: verbose proxy error handling
                continue;
            }
        }
        return false;
    }

    private HttpClient MakeHttpClient(HttpProxyConfiguration configuration)
    {
        var proxy = new WebProxy
        {
            Address = new Uri(configuration.Url),
            UseDefaultCredentials = configuration.Authentication,
        };

        if (configuration.Authentication)
        {
            proxy.Credentials = new NetworkCredential(userName: configuration.Username, password: configuration.Password) ;
        }

        var httpHandler = new HttpClientHandler
        {
            Proxy = proxy
        };

        var client = new HttpClient(httpHandler);
        return client;
    }
}
