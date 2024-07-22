using App.GrpcClient.Configurations;
using Core.Base;
using GrpcIdentityClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.GrpcClient.IdentityClient;

public class IdentityClient : 
    BaseGrpcClient<Identity.IdentityClient, IdentityClient>,
    IIdentityClient
{
    public IdentityClient(
        ILogger<IdentityClient> logger,
        IOptions<GrpcConfiguration> grpcConfiguration) : 
        base(logger, grpcConfiguration.Value.GrpcIdentityServiceUrl) { }

    public Task<GrpcAuthenticateResponse> AuthenticateAsync(GrpcAuthenticateRequest request, CancellationToken cT) =>
        Task.FromResult(Client.Authenticate(request, cancellationToken: cT));

    public Task<GrpcRegisterResponse> RegisterAsync(GrpcRegisterRequest request, CancellationToken cT) =>
        Task.FromResult(Client.Register(request, cancellationToken: cT));

    public Task<GrpcRefreshTokenResponse> RefreshTokenAsync(GrpcRefreshTokenRequest request, CancellationToken cT) =>
        Task.FromResult(Client.RefreshToken(request, cancellationToken: cT));

    public Task<GrpcRevokeResponse> RevokeAsync(GrpcRevokeRequest request, CancellationToken cT) =>
        Task.FromResult(Client.Revoke(request, cancellationToken: cT));

    public Task<GrpcRevokeAllResponse> RevokeAllAsync(GrpcRevokeAllRequest request, CancellationToken cT) =>
        Task.FromResult(Client.RevokeAll(request, cancellationToken: cT));
}