using Domain.Entities;

namespace Application.Interfaces;

public interface IGoodRepository
{
    Task<List<Good>> GetAllGoods();
}