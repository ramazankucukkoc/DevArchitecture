using AutoMapper;
using Core.Entities.Concrete;
using Core.Entities.Dtos;
using Entities.Concrete;
using Entities.Dtos;

namespace Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<Color, ColorDto>().ReverseMap();
        }
    }
}