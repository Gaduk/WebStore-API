using Application.Dto.User;
using Application.Dto.Order;
using Application.Dto.OrderedGoods;
using AutoMapper;
using Domain.Entities;

namespace Presentation;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserDto>();
        
        CreateMap<Order, OrderDto>();
        
        CreateMap<Order, OrderWithOrderedGoodsDto>();
        
        CreateMap<OrderedGood, OrderedGoodDto>()
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Good.Name))
            .ForMember(dest => dest.Price, 
                opt => opt.MapFrom(src => src.Good.Price));
        
        CreateMap<OrderedGood, OrderedGoodWithoutOrderIdDto>()
            .ForMember(dest => dest.Name, 
                opt => opt.MapFrom(src => src.Good.Name))
            .ForMember(dest => dest.Price, 
                opt => opt.MapFrom(src => src.Good.Price));
    }
}