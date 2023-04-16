using FileService.Api.Dtos.ImageDtos;
using FileService.Api.Infrastructure.Data.Abstract.Dapper;

namespace FileService.Api.Infrastructure.Data.Concrete.Dapper
{
    public class ImageRepository : IImageRepository
    {
        private readonly IConfiguration _configuration;

        public ImageRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }


    }
}
