using AutoMapper;
using FileService.Api.Entities;
using FileService.Api.Models.FileUserModels;
using FileService.Api.Models.ImageModels;

namespace FileService.Api.Mappings;

public class MapProfile : Profile
{
    public MapProfile()
    {
        CreateMap<ImageModel, Entities.Image>()
            .ForMember(i => i.FileUser, opt => opt.Ignore());

        CreateMap<Entities.Image, ImageModel>()
            .ForMember(i => i.Data, opt => opt.Ignore());

        CreateMap<FileUserModel, FileUser>()
            .ForMember(f => f.Images, opt => opt.Ignore()).ReverseMap();
    }
}