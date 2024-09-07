using Application.Dto;
using Application.Dto.OrderedGoods;
using Application.Features.OrderedGood;
using Domain.Entities;

namespace Application.Interfaces;

public interface IOrderedGoodRepository
{
    Task<List<OrderedGoodDto>> GetAllOrderedGoods();
    Task<List<OrderedGoodDto>> GetOrderedGoods(int orderId);
}