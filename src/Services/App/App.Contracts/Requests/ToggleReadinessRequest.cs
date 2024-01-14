namespace App.Contracts.Requests;

public record ToggleReadinessRequest(
    Guid RoomId,
    Guid PlayerId);