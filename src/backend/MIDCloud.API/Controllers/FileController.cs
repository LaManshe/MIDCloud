using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.API.Extensions;
using MIDCloud.API.Helpers;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.API.Controllers;

[Authorize]
[ApiController]
[Route("api/file")]
public class FileController: ControllerBase
{
    private readonly ICloudManager _cloudManager;

    public FileController(ICloudManager cloudManager)
    {
        _cloudManager = Guard.Against.Null(cloudManager, nameof(cloudManager));
    }
    
    public IActionResult GetFile(string fileName, string path)
    {
        var registeredUser = HttpContext.GetRequesterUser();

        if (registeredUser is null)
        {
            return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
        }

        var result = _cloudManager.GetFile(registeredUser, fileName, path);

        if (result.IsSuccess is false)
        {
            return BadRequest(new ResponseErrorModel(
                result.ConcatErrors()));
        }

        return Ok(result.Value);
    }
    
    [HttpGet("get-video")]
    public IActionResult GetVideoStream(string fileName, string path)
    {
        var registeredUser = HttpContext.GetRequesterUser();

        if (registeredUser is null)
        {
            return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
        }

        var videoFilePathResult = _cloudManager.GetVideoFilePath(registeredUser, fileName, path);
            
        if (videoFilePathResult.IsSuccess is false)
        {
            return BadRequest(new ResponseErrorModel(videoFilePathResult.ConcatErrors()));
        }

        return PhysicalFile(videoFilePathResult.Value, "application/octet-stream", enableRangeProcessing: true);
    }

    [HttpPost("rename")]
    public IActionResult Rename(string fileName, string newName, string path)
    {
        var registeredUser = HttpContext.GetRequesterUser();

        if (registeredUser is null)
        {
            return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
        }

        var renameFileResult = _cloudManager.RenameFile(registeredUser, fileName, newName, path);
        
        if (renameFileResult.IsSuccess is false)
        {
            return BadRequest(new ResponseErrorModel(renameFileResult.ConcatErrors()));
        }

        return Ok();
    }
}