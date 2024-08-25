using Application.Dto;
using Application.Features.OrderedGood;
using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGoodDto>> GetAllOrderedGoodDtos();
    Task<List<OrderedGoodDto>> GetOrderedGoodDtos(int orderId);
}