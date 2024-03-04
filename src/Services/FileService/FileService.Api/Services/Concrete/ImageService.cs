using FileService.Api.Dtos.ImageDtos;
using FileService.Api.Services.Abstract;
using FileService.Api.Utilities.Results;

namespace FileService.Api.Services.Concrete;

public class ImageService : IImageService
{
    public async Task<Result> AddAsync(ImageModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<Result> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByEntityTypeAndIdAsync(ImageEntityTypeAndIdModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByEntityTypeAndIdPagingAsync(ImageEntityTypeAndIdModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByEntityTypeAsync(ImageEntityTypeModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<List<ImageModel>>> GetAllByEntityTypePagingAsync(ImageEntityTypeModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<DataResult<ImageModel>> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }
}
