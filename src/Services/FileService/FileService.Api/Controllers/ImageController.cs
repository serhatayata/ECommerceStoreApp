using FileService.Api.Extensions;
using FileService.Api.Models.ImageModels;
using FileService.Api.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FileService.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ImageController : ControllerBase
{
    private readonly IImageService _imageService;

    public ImageController(
        IImageService imageService)
    {
        _imageService = imageService;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddAsync([FromForm] ImageModel model)
    {
        ///
        /// SAVE DATA TO AWS OR SOMEWHERE LIKE THAT
        /// 

        model.Path = $"blabla.com/{RandomExtensions.GetRandomNumber()}";
        var result = await _imageService.AddAsync(model);
        return Ok(result);
    }

    [HttpDelete("delete")]
    public async Task<IActionResult> DeleteAsync([FromBody] int id)
    {
        ///
        /// DELETE FROM IMAGE REPO
        ///

        var result = await _imageService.DeleteAsync(id);
        return Ok(result);
    }

    [HttpPost("get")]
    public async Task<IActionResult> GetAsync([FromBody] int id)
    {
        var result = await _imageService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("get-all-by-type-paging")]
    public async Task<IActionResult> GetAllByTypePagingAsync([FromBody] ImageTypePagingModel model)
    {
        var result = await _imageService.GetAllByTypePagingAsync(model);
        return Ok(result);
    }

    [HttpPost("get-all-by-type")]
    public async Task<IActionResult> GetAllByTypeAsync([FromBody] ImageTypeModel model)
    {
        var result = await _imageService.GetAllByTypeAsync(model);
        return Ok(result);
    }

    [HttpPost("get-by-type-file-user-id-paging")]
    public async Task<IActionResult> GetByTypeAndFileUserIdPagingAsync([FromBody] ImageTypeAndFileUserIdPagingModel model)
    {
        var result = await _imageService.GetByTypeAndFileUserIdPagingAsync(model);
        return Ok(result);
    }

    [HttpPost("get-all-by-type-file-user-id")]
    public async Task<IActionResult> GetAllByTypeAndFileUserIdAsync([FromBody] ImageTypeAndFileUserIdModel model)
    {
        var result = await _imageService.GetAllByTypeAndFileUserIdAsync(model);
        return Ok(result);
    }
}
