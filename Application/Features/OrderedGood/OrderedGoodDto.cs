namespace Application.Features.OrderedGood;

public record OrderedGoodDto(
    int OrderId, 
    int GoodId, 
    int Amount, 
    string Name, 
    float Price);