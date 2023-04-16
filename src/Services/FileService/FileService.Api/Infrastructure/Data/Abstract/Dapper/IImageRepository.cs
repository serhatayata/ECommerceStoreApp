using FileService.Api.Dtos.ImageDtos;
using Shared.Utilities.Results;

namespace FileService.Api.Infrastructure.Data.Abstract.Dapper
{
    public interface IImageRepository
    {
        /// <summary>
        /// Get all images by entity type and entity id
        /// </summary>
        /// <param name="model">model of image</param>
        /// <returns>List of images</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model);
        /// <summary>
        /// Get all images by entity type and entity id paging
        /// </summary>
        /// <param name="model">paging model</param>
        /// <returns>list of paging model</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdPagingAsync(ImageEntityTypeAndIdPagingDto model);
        /// <summary>
        /// Get all images by entity type
        /// </summary>
        /// <param name="model">image dto</param>
        /// <returns>image dto</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAsync(ImageEntityTypeDto model);
        /// <summary>
        /// Get all images by entity type and entity id paging
        /// </summary>
        /// <param name="model">paging image dto</param>
        /// <returns>image dto</returns>
        Task<DataResult<List<ImageDto>>> GetAllByEntityTypePagingAsync(ImageEntityTypePagingDto model);
        /// <summary>
        /// Get image by id
        /// </summary>
        /// <param name="id">id of the image</param>
        /// <returns>image dto</returns>
        Task<DataResult<ImageDto>> GetByIdAsync(int id);
    }
}
