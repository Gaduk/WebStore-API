namespace Application.Dto;

public record OrderedGoodDto(
    int OrderId, 
    int GoodId, 
    int Amount, 
    string Name, 
    float Price);