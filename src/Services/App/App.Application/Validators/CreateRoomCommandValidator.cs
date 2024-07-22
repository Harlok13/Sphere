using App.SignalR.Commands.LobbyCommands;
using FluentValidation;

namespace App.Application.Validators;

public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
{
    public CreateRoomCommandValidator()
    {
        RuleFor(x => x.Request.LowerBound).LessThanOrEqualTo(200);
    }
}