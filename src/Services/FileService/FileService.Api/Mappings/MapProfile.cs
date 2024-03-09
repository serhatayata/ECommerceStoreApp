using AutoMapper;
using FileService.Api.Entities;
using FileService.Api.Models.FileUserModels;
using FileService.Api.Models.ImageModels;
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

        CreateMap<FileUserModel, FileUser>()
            .ForMember(f => f.Images, opt => opt.Ignore()).ReverseMap();
    }
}