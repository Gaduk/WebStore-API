using Application.Dto.Order;
using Application.Dto.OrderedGoods;
using Application.Dto.User;
using AutoMapper;
using Domain.Entities;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        
        CreateMap<Order, OrderDto>();
        
        CreateMap<OrderedGood, OrderedGoodDto>()
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Good.Name))
            .ForMember(dest => dest.Price, 
                opt => opt.MapFrom(src => src.Good.Price));
    }
}