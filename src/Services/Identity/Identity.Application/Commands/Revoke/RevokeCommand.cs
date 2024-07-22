using Mediator;

namespace Identity.Application.Commands.Revoke;

public sealed record RevokeCommand(
    string UserName) : ICommand<bool>;