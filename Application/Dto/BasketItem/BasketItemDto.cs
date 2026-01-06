namespace Application.Dto.BasketItem;

public record BasketItemDto(
    string UserName,
    int    GoodId, 
    int    Amount);