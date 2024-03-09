using FileService.Api.Models.FileUserModels;
using FileService.Api.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileUserController : ControllerBase
{
    private readonly IFileUserService _fileUserService;

    public FileUserController(
        IFileUserService fileUserService)
    {
        _fileUserService = fileUserService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromBody] FileUserModel model)
    {
        var result = await _fileUserService.AddAsync(model);
        return Ok(result);
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateAsync([FromBody] FileUserModel model)
    {
        var result = await _fileUserService.UpdateAsync(model);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] int id)
    {
        var result = await _fileUserService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetByIdAsync([FromBody] int id)
    {
        var result = await _fileUserService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("get-by-name")]
    public async Task<IActionResult> GetByNameAsync([FromBody] string name)
    {
        var result = await _fileUserService.GetByNameAsync(name);
        return Ok(result);
    }
}
