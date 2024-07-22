using Mediator;

namespace Identity.Application.Commands.RevokeAll;

public sealed record RevokeAllCommand() : ICommand<bool>;