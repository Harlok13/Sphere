using IdentityClient;

namespace App.GrpcClient.IdentityClient;

public interface IIdentityClient
{
    Task<AuthenticateResponse> AuthenticateAsync(AuthenticateRequest request);
}