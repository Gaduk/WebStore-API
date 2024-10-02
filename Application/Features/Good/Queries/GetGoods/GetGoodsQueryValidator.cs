using FluentValidation;

namespace Application.Features.Good.Queries.GetGoods;

public class GetGoodsQueryValidator:  AbstractValidator<GetGoodsQuery>
{
    public GetGoodsQueryValidator()
    {
        RuleFor(x => x.MinPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum price must be greater than or equal to 0.");

        RuleFor(x => x.MaxPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum price must be greater than or equal to 0.");
    }
}