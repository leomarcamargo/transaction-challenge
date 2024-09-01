using FluentValidation;

namespace Transactions.Application.Commands.CreateTransaction;

public class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty()
            .WithMessage("Date is required");

        RuleFor(x => x.Total)
            .GreaterThan(0)
            .WithMessage("Total must be greater than zero");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Type must be Credit or Debit");

        RuleFor(x => x.Description)
            .MaximumLength(250)
            .WithMessage("Description cannot exceed 250 characters");
    }
}