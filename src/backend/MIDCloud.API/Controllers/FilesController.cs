using Ardalis.GuardClauses;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.API.Extensions;
using MIDCloud.API.Models.ResponseModels;
using MIDCloud.API.Helpers;
using MIDCloud.GlobalInterfaces.Services;
using MIDCloud.API.Models.FileModels;

namespace MIDCloud.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/files")]
    public class FilesController : ControllerBase
    {
        private readonly ICloudManager _cloudManager;

        public FilesController(ICloudManager cloudManager)
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
                    "You need to use Authentication with token to enable this feature"));
            }

            var tilesResult = _cloudManager.GetTilesOfDirectory(registeredUser, path);

            if (tilesResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(
                    tilesResult.ConcatErrors()));
            }

            return Ok(tilesResult.Value);
        }

        [HttpGet("limited")]
        public async Task<IActionResult> Get(string path, string limit, string page)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel(
                    "You need to use Authentification with token to enable this feature"));
            }

            var tilesResult = _cloudManager.GetTilesOfDirectoryLimited(registeredUser, path, limit, page);

            var maxPage = _cloudManager.GetMaxPage(registeredUser, path, limit, page);

            Response.SetResponseMaxPageHeader(maxPage.Value.ToString());

            if (tilesResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(
                    tilesResult.ConcatErrors()));
            }

            return Ok(tilesResult.Value);
        }

        [HttpGet("limited-and-sorted")]
        public IActionResult GetSorted(string path, string limit, string page, string sortType)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel(
                    "You need to use Authentification with token to enable this feature"));
            }

            var tilesResult = _cloudManager
                .GetTilesOfDirectoryLimited(
                    registeredUser, path, 
                    limit, page);
            
            tilesResult.Value.SortFiles(StringToEnumConverter.GetSortFileType(sortType));

            var maxPage = _cloudManager.GetMaxPage(registeredUser, path, limit, page);

            Response.SetResponseMaxPageHeader(maxPage.Value.ToString());

            if (tilesResult.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(
                    tilesResult.ConcatErrors()));
            }

            return Ok(tilesResult.Value);
        }

        [DisableRequestSizeLimit]
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
                return BadRequest(new ResponseErrorModel(result.ConcatErrors()));
            }

            return Ok(result.Value);
        }

        [HttpPost("delete-folders")]
        public IActionResult DeleteFolders([FromBody] DeleteFoldersRequest foldersDeleteRequest)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
            }

            var result = _cloudManager.UnregisterFolders(registeredUser, foldersDeleteRequest.Folders);

            if (result.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(result.ConcatErrors()));
            }

            return Ok(result.Value);
        }

        [HttpPost("delete-files")]
        public IActionResult DeleteFiles([FromBody] DeleteFilesRequest deleteFilesRequest)
        {
            var registeredUser = HttpContext.GetRequesterUser();

            if (registeredUser is null)
            {
                return BadRequest(new ResponseErrorModel("You need to use Basic Authentification with Login and Password to enable this feature"));
            }

            var result = _cloudManager.RemoveFiles(registeredUser, deleteFilesRequest.Files);

            if (result.IsSuccess is false)
            {
                return BadRequest(new ResponseErrorModel(result.ConcatErrors()));
            }

            return Ok(result.Value);
        }
    }
}
 