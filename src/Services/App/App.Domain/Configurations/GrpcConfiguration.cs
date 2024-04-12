namespace App.Domain.Configurations;

public class GrpcConfiguration
{
    public string GrpcUserInteractionServiceUrl { get; init; } = null!;
    public string GrpcIdentityServiceUrl { get; init; } = null!;
}
