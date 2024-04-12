using App.Domain.Entities.RoomEntity;

namespace App.Domain.Tests.Unit;

public class RoomTests
{
    // [Theory]
    // [InlineData(null)]
    // [InlineData("")]
    // public void Create_Should_ReturnNull_WhenValueLengthIsInvalid(string? value)
    // {
    //     // Arrange
    //
    //     // Act
    //     var room = Room.Create(value);
    //
    //     // Assert
    //     Assert.Null(room);
    // }
    //
    // public static IEnumerable<object[]> InvalidRoomLengthData => new List<object[]>()
    // {
    //     new object[] { "invalid length" },
    //     new object[] { "invalid length2" },
    //     new object[] { "invalid length3" }
    // };
    //
    // [Theory]
    // [MemberData(nameof(InvalidRoomLengthData))]
    // public void Create_Should_ReturnNull_WhenValueLengthIsInvalid2(string? value)
    // {
    //     // Arrange
    //
    //     // Act
    //     var room = Room.Create(value);
    //
    //     // Assert
    //     Assert.Null(room);
    // }

    [Theory]
    [ClassData(typeof(RoomCreateValidTestData))]
    public void Create_Should_ReturnRoom_WhenReceiveValidData(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl,
        int lowerStartMoneyBound,
        int upperStartMoneyBound)
    {
        // Arrange

        // Act
        var room = Room.Create(
            id: id, 
            roomName: roomName,
            roomSize: roomSize,
            startBid: startBid,
            minBid: minBid,
            maxBid: maxBid,
            avatarUrl: avatarUrl,
            lowerStartMoneyBound: lowerStartMoneyBound,
            upperStartMoneyBound: upperStartMoneyBound);

        // Assert
        Assert.Equal(id, room.Id);
        Assert.Equal(roomName, room.RoomName);
        Assert.Equal(roomSize, room.RoomSize);
        Assert.Equal(startBid, room.StartBid);
        Assert.Equal(minBid, room.MinBid);
        Assert.Equal(maxBid, room.MaxBid);
        Assert.Equal(avatarUrl, room.AvatarUrl);
        Assert.Equal(lowerStartMoneyBound, room.LowerStartMoneyBound);
        Assert.Equal(upperStartMoneyBound, room.UpperStartMoneyBound);
        Assert.Equal(Room.DefaultRoomStatus, room.Status);
        Assert.Equal(Room.DefaultPlayersInRoom, room.PlayersInRoom);
        Assert.Equal(default, room.Bank);
    }

    [Theory]
    [ClassData(typeof(RoomCreateInvalidTestData))]
    public void Create_Should_ReturnErrorDomainResult_WhenReceiveInvalidData(
        Guid id,
        string roomName,
        int roomSize,
        int startBid,
        int minBid,
        int maxBid,
        string avatarUrl,
        int lowerStartMoneyBound,
        int upperStartMoneyBound)
    {
        var room = Room.Create(
            id: id, 
            roomName: roomName,
            roomSize: roomSize,
            startBid: startBid,
            minBid: minBid,
            maxBid: maxBid,
            avatarUrl: avatarUrl,
            lowerStartMoneyBound: lowerStartMoneyBound,
            upperStartMoneyBound: upperStartMoneyBound);
    }
}

public class RoomCreateValidTestData : TheoryData<Guid, string, int, int, int, int, string, int, int>
{
    public RoomCreateValidTestData()
    {
        Add(Guid.NewGuid(), "TestRoomName", 3, 100, 200, 444, "/img/ava.png", 30, 100);
        // Add(Guid.NewGuid(), "TestRoomName3", 3, 100, 200, 444, "/img/ava.png", 30, 100);
    }
}

internal class RoomData
{
    public RoomData()
    {
        
    }
}

public class RoomCreateInvalidTestData : TheoryData<Guid, string, int, int, int, int, string, int, int>
{
    public RoomCreateInvalidTestData()
    {
        var invalidRoomNameLength = "VeryLongLine with the name of the room";
        Add(Guid.NewGuid(), invalidRoomNameLength, 3, 100, 200, 444, "/img/ava.png", 30, 100);
    }
}
//
// public static Room Create(
//     Guid id,
//     string roomName,
//     int roomSize,
//     int startBid,
//     int minBid,
//     int maxBid,
//     string avatarUrl,
//     int lowerStartMoneyBound,
//     int upperStartMoneyBound)
// {
//     var room = new Room(
//         id: id,
//         roomName: roomName,
//         roomSize: roomSize,
//         startBid: startBid,
//         minBid: minBid,
//         maxBid: maxBid,
//         avatarUrl: avatarUrl,
//         lowerStartMoneyBound: lowerStartMoneyBound,
//         upperStartMoneyBound: upperStartMoneyBound);
//         
//     room._domainEvents.Add(new CreatedRoomDomainEvent(room));
//     return room;
// }