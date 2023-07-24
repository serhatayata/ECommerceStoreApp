using AutoMapper;
using CatalogService.Api.Entities;
using CatalogService.Api.Models.Base.Concrete;
using CatalogService.Api.Utilities.Results;

namespace CatalogService.Api.Mapping;

public class MapProfile : Profile
{
    public MapProfile()
    {
        #region GENERAL
        CreateMap<IntModel, GrpcIntModel>().ReverseMap();
        CreateMap<StringModel, GrpcStringModel>().ReverseMap();
        CreateMap<BoolModel, GrpcBoolModel>().ReverseMap();
        CreateMap<PagingModel, GrpcPagingModel>().ReverseMap();


        CreateMap<Result, GrpcResponseModel>().ReverseMap();
        #endregion

        #region BRAND
        CreateMap<Brand, GrpcBrand>().ReverseMap();
        CreateMap<IEnumerable<Brand>, ListGrpcBrand>()
                .ForMember(dest => dest.Brands, opt => opt.MapFrom(s => s));

        CreateMap<Brand, GrpcBrandModel>().ReverseMap();
        CreateMap<IEnumerable<Brand>, ListGrpcBrandModel>()
                .ForMember(dest => dest.Brands, opt => opt.MapFrom(s => s));
        #endregion
        #region PRODUCT
        CreateMap<GrpcProduct, Product>().ReverseMap();
        CreateMap<GrpcProductModel, Product>().ReverseMap();
        CreateMap<IEnumerable<Product>, ListGrpcProductModel>()
            .ForMember(dest => dest.Products, opt => opt.MapFrom(s => s));

        CreateMap<ProductType, GrpcProductType>().ReverseMap();

        CreateMap<ProductFeature, GrpcProductFeature>().ReverseMap();
        CreateMap<ProductFeature, GrpcProductFeatureModel>().ReverseMap();
        CreateMap<ProductFeatureProperty, GrpcProductFeaturePropertyModel>().ReverseMap();

        CreateMap<IEnumerable<ProductFeatureProperty>, ListGrpcProductFeaturePropertyModel>()
            .ForMember(dest => dest.ProductFeatureProperties, opt => opt.MapFrom(s => s));
        #endregion
        #region CATEGORY
        CreateMap<Category, GrpcCategory>().ReverseMap();
        CreateMap<IEnumerable<Category>, ListGrpcCategory>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(s => s));

        CreateMap<Category, GrpcCategoryModel>().ReverseMap();
        CreateMap<IEnumerable<Category>, ListGrpcCategoryModel>()
            .ForMember(dest => dest.Categories, opt => opt.MapFrom(s => s));
        #endregion
        #region COMMENT
        CreateMap<Comment, GrpcComment>().ReverseMap();
        CreateMap<IEnumerable<Comment>, ListGrpcComment>()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(s => s));

        CreateMap<Comment, GrpcCommentModel>().ReverseMap();
        CreateMap<IEnumerable<Comment>, ListGrpcCommentModel>()
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(s => s));
        #endregion
        #region FEATURE
        CreateMap<Feature, GrpcFeature>().ReverseMap();
        CreateMap<IEnumerable<Feature>, ListGrpcFeature>()
            .ForMember(dest => dest.Features, opt => opt.MapFrom(s => s));

        CreateMap<Feature, GrpcFeatureModel>().ReverseMap();
        CreateMap<IEnumerable<Feature>, ListGrpcFeatureModel>()
            .ForMember(dest => dest.Features, opt => opt.MapFrom(s => s));
        #endregion
    }
}
