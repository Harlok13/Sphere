using App.Domain.Configurations;
using App.GrpcClient.Base;
using IdentityClient;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace App.GrpcClient.IdentityClient;

public class IdentityClient : 
    BaseClient<Identity.IdentityClient, IdentityClient>,
    IIdentityClient
{
    public IdentityClient(
        ILogger<IdentityClient> logger,
        IOptions<GrpcConfiguration> grpcConfiguration) : 
        base(logger, grpcConfiguration.Value.GrpcIdentityServiceUrl) { }

    public Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request)
        => Task.FromResult(Client.Authenticate(request));

    public Task<RegisterResponse> RegisterAsync(RegisterRequest request)
        => Task.FromResult(Client.Register(request));

    public Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
        => Task.FromResult(Client.RefreshToken(request));

    public Task<RevokeResponse> RevokeAsync(RevokeRequest request)
        => Task.FromResult(Client.Revoke(request));

    public Task<RevokeAllResponse> RevokeAllAsync(RevokeAllRequest request)
        => Task.FromResult(Client.RevokeAll(request));
}