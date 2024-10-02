using FluentValidation;

namespace Application.Features.Order.Commands.CreateOrder;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty().WithMessage("User name is required")
            .MaximumLength(30).WithMessage("User name must not exceed 30 characters");
        
        RuleFor(x => x.OrderedGoods)
            .NotEmpty().WithMessage("At least one ordered good is required")
            .Must(x => x.All(og => og.GoodId > 0))
            .WithMessage("Good id must be greater than 0")
            .Must(x => x.All(og => og.Amount > 0))
            .WithMessage("Amount must be greater than 0");
    }
}

