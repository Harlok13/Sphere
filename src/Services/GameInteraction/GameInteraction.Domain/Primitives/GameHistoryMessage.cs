namespace GameInteraction.Domain.Primitives;

public record GameHistoryMessage(
    string Type,
    string CurrentTime,
    string Message,
    string? PlayerName = default);