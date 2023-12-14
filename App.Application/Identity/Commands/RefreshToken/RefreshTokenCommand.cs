using App.Contracts.Identity.Requests;
using App.Contracts.Identity.Responses;
using Mediator;

namespace App.Application.Identity.Commands.RefreshToken;

public sealed record RefreshTokenCommand(RefreshTokenRequest TokenRequest) : ICommand<RefreshTokenResponse>;