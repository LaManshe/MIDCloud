using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.API.Extensions;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.API.Helpers;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.API.Models.FileModels;
using MIDCloud.GlobalInterfaces.Requests;

namespace MIDCloud.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly ICloudManager _cloudManager;

        public FileController(ICloudManager cloudManager)
        {
            _cloudManager = Guard.Against.Null(cloudManager, nameof(cloudManager));
        }

        [HttpGet]
        public IActionResult Get(string path)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel(
                    "You need to use Authentification with token to enable this feature"));
            }

            var tilesResult = _cloudManager.GetTilesOfDirectory(registeredUser, path);

            //var filesFromFolderResult = _cloudManager.GetTiles(string.Empty, registeredUser);

            if (tilesResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(
                    tilesResult.ConcatErrors()));
            }

            return Ok(tilesResult.Value);
        }

        [HttpPost("upload")]
        public IActionResult Upload([FromForm] UploadFilesModel uploadModel)
        {
            var files = uploadModel.Files;
            var folder = uploadModel.Folder;

            if (files is null || files.Any() is false)
            {
                return BadRequest(new ResponseErrorModel("No files upload"));
            }

            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
            }

            var result = _cloudManager.UploadFiles(registeredUser, folder, files);

            if (result.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(result.ConcatErrors()));
            }

            return Ok();
        }

        [HttpPost("create-folder")]
        public IActionResult CreateFolder([FromForm] string folderPath)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
            }

            var result = _cloudManager.RegisterFolder(registeredUser, folderPath);

            if (result.IsSuccess is false)
            {
                return BadRequest(result.ConcatErrors());
            }

            return Ok(result.Value);
        }

        //[HttpPost("delete-folder")]
        //public IActionResult DeleteFolder([FromForm] string folderPath)
        //{
        //    var registeredUser = HttpContext.GetRequesterUser();

        //    if (registeredUser is null)
        //    {
        //        return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
        //    }

        //    var resultDeletingFolder = _cloudManager.DeleteFolder(folderPath, registeredUser);

        //    if (resultDeletingFolder.IsSuccess is false)
        //    {
        //        return BadRequest(new ResponseErrorModel(resultDeletingFolder.ConcatErrors()));
        //    }

        //    return Ok(resultDeletingFolder.Value);
        //}
    }
}
 