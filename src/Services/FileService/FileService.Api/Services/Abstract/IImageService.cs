using FileService.Api.Models.ImageModels;
using FileService.Api.Utilities.Results;

namespace FileService.Api.Services.Abstract;

public interface IImageService
{
    /// <summary>
    /// Adds an image
    /// </summary>
    /// <param name="model">image dto</param>
    /// <returns>Url value</returns>
    Task<Result> AddAsync(ImageModel model);

    /// <summary>
    /// Get image by id
    /// </summary>
    /// <param name="id">image id</param>
    /// <returns>image data</returns>
    Task<DataResult<ImageModel>> GetByIdAsync(int id);

    /// <summary>
    /// Get all images by entity type
    /// </summary>
    /// <param name="model">image get dto</param>
    /// <returns></returns>
    Task<DataResult<List<ImageModel>>> GetAllByTypeAsync(ImageTypeModel model);

    /// <summary>
    /// Get all images paging by entity type
    /// </summary>
    /// <param name="model">image get dto</param>
    /// <returns></returns>
    Task<DataResult<List<ImageModel>>> GetAllByTypePagingAsync(ImageTypePagingModel model);

    /// <summary>
    /// Get all images by entity type and id
    /// </summary>
    /// <param name="model">image dto</param>
    /// <returns>List of image dto</returns>
    Task<DataResult<List<ImageModel>>> GetAllByTypeAndFileUserIdAsync(ImageTypeAndFileUserIdModel model);

    /// <summary>
    /// Get all images paging by entity type and id
    /// </summary>
    /// <param name="model">image dto</param>
    /// <returns>List of image dto</returns>
    Task<DataResult<List<ImageModel>>> GetByTypeAndFileUserIdPagingAsync(ImageTypeAndFileUserIdPagingModel model);

    /// <summary>
    /// Delete image by id
    /// </summary>
    /// <param name="id">id of the image</param>
    /// <returns>If the image deleted or not</returns>
    Task<Result> DeleteAsync(int id);
}
