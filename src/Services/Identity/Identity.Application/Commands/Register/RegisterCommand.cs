using Identity.Contracts.Requests;
using Identity.Contracts.Responses;
using Mediator;

namespace Identity.Application.Commands.Register;

public sealed record RegisterCommand(
    RegisterRequest RegisterRequest) : ICommand<AuthenticateResponse>;