namespace App.GrpcClient.Configurations;

public class GrpcConfiguration
{
    public string GrpcIdentityServiceUrl { get; init; } = null!;
    public string GrpcUserInteractionServiceUrl { get; init; } = null!;
    public string GrpcGameInteractionServiceUrl { get; init; } = null!;
    // public string Host { get; set; }
    // public string Port { get; set; }
    // public string Scheme { get; set; }
}
