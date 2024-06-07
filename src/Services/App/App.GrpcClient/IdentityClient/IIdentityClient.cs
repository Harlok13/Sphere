using GrpcIdentityClient;

namespace App.GrpcClient.IdentityClient;

public interface IIdentityClient
{
    Task<GrpcAuthenticateResponse> AuthenticateAsync(GrpcAuthenticateRequest request, CancellationToken cT);

    Task<GrpcRegisterResponse> RegisterAsync(GrpcRegisterRequest request, CancellationToken cT);

    Task<GrpcRefreshTokenResponse> RefreshTokenAsync(GrpcRefreshTokenRequest request, CancellationToken cT);

    Task<GrpcRevokeResponse> RevokeAsync(GrpcRevokeRequest request, CancellationToken cT);

    Task<GrpcRevokeAllResponse> RevokeAllAsync(GrpcRevokeAllRequest request, CancellationToken cT);
}