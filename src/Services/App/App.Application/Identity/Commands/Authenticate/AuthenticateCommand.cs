using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using Mediator;

namespace App.Application.Identity.Commands.Authenticate;

public sealed record AuthenticateCommand(
    AuthenticateRequest AuthenticateRequest) : ICommand<AuthenticateResponse>;
