namespace BlogAtor.API.Controllers;

using BlogAtor.Collector.Configuration;
using BlogAtor.Core.Config;
using BlogAtor.Store.Abstrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfigStore _store;

    public ConfigController(IConfigStore configStore)
    {
        _store = configStore;
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<APIResponse<System.Object>>> GetConfigById(System.Int64 id)
    {
        var config = await _store.GetConfigById(id);
        if (config == null)
        {
            var errorInfo = new ErrorInfo("ConfigNotFound");
            return NotFound(APIResponse.Error<System.Object>(errorInfo));
        }
        return Ok(APIResponse.Success<System.Object>(config));
    }
    [HttpGet("collector/rss")]
    public async Task<ActionResult<APIResponse<ICollection<ConfigBase<RssCollectorConfiguration>>>>> GetRssConfig()
    {
        var result = await _store.GetConfigurationsOfType<RssCollectorConfiguration>();
        return Ok(APIResponse.Success(result));
    }
    [HttpPost("collector/rss")]
    [ProducesResponseType<APIResponse<System.Int64>>(StatusCodes.Status200OK)]
    public async Task<ActionResult<APIResponse<System.Int64>>> SaveRssConfig([FromBody] ConfigBase<RssCollectorConfiguration> config)
    {
        var configId = await _store.SaveConfiguration(config);
        return Ok(APIResponse.Success(configId));
    }
}