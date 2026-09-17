namespace BlogAtor.API.Controllers;

using BlogAtor.Collector.Abstractions;
using BlogAtor.Collector.Configuration;
using BlogAtor.Core.Config;
using BlogAtor.Store.Abstrations;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProxyController : ControllerBase
{
    private readonly IConfigStore _store;
    private readonly IHttpClientProvider _httpClientProvider;
    public ProxyController(IConfigStore configStore, IHttpClientProvider httpClientProvider)
    {
        _store = configStore;
        _httpClientProvider = httpClientProvider;
    }
    [HttpPost]
    [ProducesResponseType<APIResponse<ConfigBase<HttpProxyConfiguration>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ConfigBase<HttpProxyConfiguration>>>> AddProxyConfig([FromBody] HttpProxyConfiguration configuration)
    {
        var config = new ConfigBase<HttpProxyConfiguration>()
        {
            Configuration = configuration
        };

        var check = await _httpClientProvider.ValidateHttpProxy(configuration);
        if (!check)
        {
            var errorInfo = new ErrorInfo("ErrorInvalidProxyParameters");
            return BadRequest(APIResponse.Error<ConfigBase<HttpProxyConfiguration>>(errorInfo));
        }

        var id = await _store.SaveConfiguration(config);
        config.Id = id;
        return APIResponse.Success(config);
    }
    [HttpDelete("{id}")]
    [ProducesResponseType<APIResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<APIResponse>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<APIResponse>> DeleteProxyConfig(System.Int64 id)
    {
        if (await _store.DeleteConfigById(id))
        {
            return APIResponse.Success();
        }
        var errorInfo = new ErrorInfo($"ConfigIdNotFound");
        return APIResponse.Error(errorInfo);
    }
    [HttpGet]
    [ProducesResponseType<APIResponse<ICollection<HttpProxyConfiguration>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<ICollection<ConfigBase<HttpProxyConfiguration>>>>> GetProxyConfigurations()
    {
        var result = await _store.GetConfigurationsOfType<HttpProxyConfiguration>();
        foreach (var config in result)
        {
            if (config.Configuration.Password != null)
            {
                config.Configuration.Password = System.String.Empty;
            }
        }
        return APIResponse.Success(result);
    }
    [HttpPost("search")]
    [ProducesResponseType<APIResponse<SearchResult<ConfigBase<HttpProxyConfiguration>>>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<SearchResult<ConfigBase<HttpProxyConfiguration>>>>> SearchAsync([FromBody] SearchOptions options)
    {
        var result = await _store.GetConfigurationsOfType<HttpProxyConfiguration>();
        var filteredResults = result;
        if (options.Search.Value != System.String.Empty)
        {
            filteredResults = result.Where(cb => cb.Configuration.Url.Contains(options.Search.Value)
                                                || (cb.Configuration.Username?.Contains(options.Search.Value) ?? false)).ToList();
        }
        foreach (var filtered in filteredResults)
        {
            filtered.Configuration.Password = System.String.Empty;
        }
        return Ok(APIResponse.Success(filteredResults.ToSearchResult(options.Draw, result.Count(), filteredResults.Count())));
    }
}