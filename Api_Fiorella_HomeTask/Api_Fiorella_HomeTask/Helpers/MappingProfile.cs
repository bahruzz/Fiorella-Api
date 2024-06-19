using Api_Fiorella_HomeTask.DTOs.Blogs;
using Api_Fiorella_HomeTask.DTOs.Categories;
using Api_Fiorella_HomeTask.DTOs.Products;
using Api_Fiorella_HomeTask.DTOs.Sliders;
using Api_Fiorella_HomeTask.Models;
using AutoMapper;

namespace Api_Fiorella_HomeTask.Helpers
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryDto>();

            CreateMap<CategoryCreateDto, Category>();

            CreateMap<CategoryEditDto, Category>();
            CreateMap<Blog, BlogDto>();

            CreateMap<BlogCreateDto, Blog>();

            CreateMap<BlogEditDto, Blog>();
            CreateMap<Slider, SliderDto>();

            CreateMap<SliderCreateDto, Slider>();

            CreateMap<SliderEditDto, Slider>();
            CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name))
              .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ProductImages.FirstOrDefault()));
        }

    }
}
