using AutoMapper;
using FileService.Api.Dtos.ImageDtos;
using FileService.Api.Utilities.Convertions;

namespace FileService.Api.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<ImageModel, Entities.Image>()
            .ForMember(i => i.Data, opt => opt.MapFrom(s => s.Data.GetBytes()));

        CreateMap<Entities.Image, ImageModel>()
            .ForMember(i => i.Data, opt => opt.MapFrom(s => s.Data.ToFormFile($"{s.FileUserId}-{s.EntityId}")));
    }
}