using Mediator;

namespace App.Application.Identity.Commands.Revoke;

public sealed record RevokeCommand(string UserName) : ICommand<bool>;