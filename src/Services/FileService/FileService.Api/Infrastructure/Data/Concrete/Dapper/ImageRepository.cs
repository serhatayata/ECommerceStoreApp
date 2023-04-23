using FileService.Api.Dtos.ImageDtos;
using FileService.Api.Infrastructure.Data.Abstract.Dapper;
using Shared.Utilities.Results;

namespace FileService.Api.Infrastructure.Data.Concrete.Dapper
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdAsync(ImageEntityTypeAndIdDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAndIdPagingAsync(ImageEntityTypeAndIdPagingDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypeAsync(ImageEntityTypeDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<List<ImageDto>>> GetAllByEntityTypePagingAsync(ImageEntityTypePagingDto model)
        {
            throw new NotImplementedException();
        }

        public Task<DataResult<ImageDto>> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
