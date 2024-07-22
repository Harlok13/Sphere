// using FluentValidation;
//
// namespace App.Application.Validators;
//
// public class CreateMemberCommandValidator : AbstractValidator<CreateMemberCommand>
// {
//     public CreateMemberCommandValidator()
//     {
//         RuleFor(x => x.Email).NotEmpty();
//
//         RuleFor(x => x.FirstName).NotEmpty().MaximumLength(FirstName.MaxLength);
//
//         RuleFor(x => x.LastName).NotEmpty().MaximumLength(LastName.MaxLength);
//     }
// }