namespace BlogAtor.Collector.Abstractions;

using BlogAtor.Collector.Configuration;

public interface IHttpClientProvider
{
    public Task<System.Boolean> ValidateHttpProxy(HttpProxyConfiguration configuration);
    public Task<ICollection<System.Int64>> ListWebProxyIds();
    public Task<HttpClient?> GetProxyClient(System.Int64 proxyId);
}