using Mediator;

namespace App.Application.Identity.Commands.RevokeAll;

public sealed record RevokeAllCommand() : ICommand<bool>;