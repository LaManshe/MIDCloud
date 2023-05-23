using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using MIDCloud.FilesManager.Services.Interfaces;

namespace MIDCloud.FilesManager.Controllers;

[Route("files-manager/files")]
[ApiController]
public class FilesController : ControllerBase
{
    private readonly IExplorerManager _fileExplorer;

    public FilesController(IExplorerManager fileExplorer)
    {
        _fileExplorer = fileExplorer;
    }

    [HttpGet("get-file")]
    public IActionResult GetFile(string filePath)
    {
        try
        {
            var file = _fileExplorer.FileManager.Get(filePath);

            return Ok(file);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("get-files")]
    public IActionResult GetFilesOfDirectory(string dirPath)
    {
        try
        {
            var files = _fileExplorer.GetFilesOfDirectory(dirPath);

            return Ok(files);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("upload")]
    public IActionResult UploadFileIn([FromForm] IFormFile file, [FromForm] string destination)
    {
        try
        {
            _fileExplorer.FileManager.Upload(file, destination);

            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("rename")]
    public IActionResult RenameFile(string filePath, string newName)
    {
        try
        {
            _fileExplorer.FileManager.Rename(filePath, newName);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("delete")]
    public IActionResult DeleteFile(string filePath)
    {
        try
        {
            _fileExplorer.FileManager.Delete(filePath);
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("move")]
    public IActionResult MovesTo(List<string> files, string destination)
    {
        try
        {
            files.ForEach(file => 
                _fileExplorer.FileManager.Move(file, destination));
            
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}