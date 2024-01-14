using App.Domain.DomainResults;
using App.Domain.Primitives;

namespace App.Domain.ValueObjects;

public sealed class RoomName : ValueObject
{
    private const int MaxLength = 30;

    private RoomName(string value) => Value = value;

    public string Value { get; }

    public static DomainResult Create(string roomName)
    {
        if (string.IsNullOrWhiteSpace(roomName))
            return new DomainFailure("");

        if (roomName.Length > MaxLength)
            return new DomainFailure("");

        return new DomainSuccessResult<RoomName>(new RoomName(roomName));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
}