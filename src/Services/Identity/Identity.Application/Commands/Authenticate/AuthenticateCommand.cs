using Identity.Contracts.Requests;
using Identity.Contracts.Responses;
using Mediator;

namespace Identity.Application.Commands.Authenticate;

public sealed record AuthenticateCommand(
    AuthenticateRequest AuthenticateRequest) : ICommand<AuthenticateResponse>;
