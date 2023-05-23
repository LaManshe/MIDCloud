using Microsoft.AspNetCore.Mvc;
using MIDCloud.API.Extensions;
using MIDCloud.API.Helpers;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.GlobalInterfaces.Services;

namespace MIDCloud.API.Controllers;

[Authorize]
[ApiController]
[Route("api/folders")]
public class FoldersController: ControllerBase
{
    private readonly ICloudManager _cloudManager;

    public FoldersController(ICloudManager cloudManager)
    {
        _cloudManager = cloudManager;
    }

    [HttpGet("folder-tree")]
    public IActionResult GetFolderTree(string startPath)
    {
        var registeredUser = HttpContext.GetRequesterUser();

        if (registeredUser is null)
        {
            return BadRequest(new ResponseErrorModel(
                "You need to use Authentication with token to enable this feature"));
        }

        var foldersTreeResult = _cloudManager.GetFoldersTree(registeredUser, startPath);
        
        if (foldersTreeResult.IsSuccess is false)
        {
            return BadRequest(new ResponseErrorModel(
                foldersTreeResult.ConcatErrors()));
        }

        return Ok(foldersTreeResult.Value);
    }
}