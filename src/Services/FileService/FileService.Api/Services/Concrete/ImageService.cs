using FileService.Api.Dtos.ImageDtos;
using FileService.Api.Services.Abstract;
using Shared.Utilities.Results;

namespace FileService.Api.Services.Concrete
{
    public class ImageService : IImageService
    {
        public Task<DataResult<string>> AddAsync(ImageDto model)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Result> DeleteByUrlAsync(string url)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdPagingAsync(ImageEntityTypeAndIdDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAsync(ImageEntityTypeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypePagingAsync(ImageEntityTypeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<ImageDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
