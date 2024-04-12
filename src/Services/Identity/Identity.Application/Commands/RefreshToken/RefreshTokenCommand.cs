using Identity.Contracts.Requests;
using Identity.Contracts.Responses;
using Mediator;

namespace Identity.Application.Commands.RefreshToken;

public sealed record RefreshTokenCommand(
    RefreshTokenRequest TokenRequest) : ICommand<RefreshTokenResponse>;