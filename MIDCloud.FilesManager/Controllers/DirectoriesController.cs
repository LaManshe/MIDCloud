using Microsoft.AspNetCore.Mvc;
using MIDCloud.FilesManager.Services.Interfaces;

namespace MIDCloud.FilesManager.Controllers;

[Route("files-manager/directories")]
[ApiController]
public class DirectoriesController : ControllerBase
{
    private readonly IExplorerManager _fileExplorer;

    public DirectoriesController(IExplorerManager fileExplorer)
    {
        _fileExplorer = fileExplorer;
    }

    [HttpGet("get")]
    public IActionResult Get(string dirPath)
    {
        try
        {
            var directories = _fileExplorer.GetDirectoriesOfDirectory(dirPath);

            return Ok(directories);
        }
        catch (Exception e)
        {
            return BadRequest();
        }
    }
}