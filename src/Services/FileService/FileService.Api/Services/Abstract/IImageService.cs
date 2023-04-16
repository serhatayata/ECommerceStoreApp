using FileService.Api.Dtos.ImageDtos;
using Shared.Utilities.Results;

namespace FileService.Api.Services.Abstract
{
    public interface IImageService
    {
        /// <summary>
        /// Adds an image
        /// </summary>
        /// <param name="model">image dto</param>
        /// <returns>Url value</returns>
        Task<DataResult<string>> AddAsync(ImageDto model);
        /// <summary>
        /// Get image by id
        /// </summary>
        /// <param name="id">image id</param>
        /// <returns>image data</returns>
        Task<DataResult<ImageDto>> GetByIdAsync(int id);
        /// <summary>
        /// Get all images by entity type
        /// </summary>
        /// <param name="model">image get dto</param>
        /// <returns></returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAsync(ImageEntityTypeDto model);
        /// <summary>
        /// Get all images paging by entity type
        /// </summary>
        /// <param name="model">image get dto</param>
        /// <returns></returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypePagingAsync(ImageEntityTypeDto model);
        /// <summary>
        /// Get all images by entity type and id
        /// </summary>
        /// <param name="model">image dto</param>
        /// <returns>List of image dto</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model);
        /// <summary>
        /// Get all images paging by entity type and id
        /// </summary>
        /// <param name="model">image dto</param>
        /// <returns>List of image dto</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdPagingAsync(ImageEntityTypeAndIdDto model);
        /// <summary>
        /// Delete image by id
        /// </summary>
        /// <param name="id">id of the image</param>
        /// <returns>If the image deleted or not</returns>
        Task<Result> DeleteByIdAsync(int id);
        /// <summary>
        /// Delete an image by entity type and id
        /// </summary>
        /// <param name="model">image dto</param>
        /// <returns>if the image deleted or not</returns>
        Task<Result> DeleteByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model);
        /// <summary>
        /// Delete image by url
        /// </summary>
        /// <param name="url">image url</param>
        /// <returns>if the image deleted or not</returns>
        Task<Result> DeleteByUrlAsync(string url);
    }
}
