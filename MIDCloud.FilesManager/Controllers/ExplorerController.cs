using Microsoft.AspNetCore.Mvc;
using MIDCloud.FilesManager.Models;
using MIDCloud.FilesManager.Services.Interfaces;

namespace MIDCloud.FilesManager.Controllers;

[Route("files-manager/explorer")]
[ApiController]
public class ExplorerController : ControllerBase
{
    private readonly IExplorerManager _fileExplorer;

    public ExplorerController(IExplorerManager fileExplorer)
    {
        _fileExplorer = fileExplorer;
    }
    
    [HttpGet("get")]
    public IActionResult Get(string dirPath)
    {
        try
        {
            var includes = _fileExplorer.GetIncludesOfDirectory(dirPath);

            return Ok(includes);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}