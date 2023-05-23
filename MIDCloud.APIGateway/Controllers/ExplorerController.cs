using Microsoft.AspNetCore.Mvc;

namespace MIDCloud.APIGateway.Controllers;

[Route("explorer")]
[ApiController]
public class ExplorerController : ControllerBase
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ExplorerController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    [HttpGet("get")]
    public async Task<IActionResult> GetFilesOfDirectory(string dirPath)
    {
        var filesManager = _httpClientFactory.CreateClient("FileManager");

        var filesResponse = await
            filesManager.GetAsync($"files-manager/explorer/get?dirPath={dirPath}");

        if (!filesResponse.IsSuccessStatusCode)
        {
            return BadRequest();
        }
        
        var filesJson = await filesResponse.Content.ReadAsStringAsync();

        return Ok(filesJson);
    }
}