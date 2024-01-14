using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using Mediator;

namespace App.Application.Identity.Commands.Register;

public sealed record RegisterCommand(RegisterRequest RegisterRequest) : ICommand<AuthenticateResponse>;