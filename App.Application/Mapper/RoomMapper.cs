using App.Contracts.Requests;
using App.Contracts.Responses;
using App.Domain.Entities;
using App.Domain.Enums;

namespace App.Application.Mapper;

public class RoomMapper
{
    public static RoomResponse MapRoomToRoomResponse(Room entity, IEnumerable<PlayerResponse> playersResponse)
    {
        return new RoomResponse(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid,
            ImgUrl: entity.AvatarUrl,
            Status: entity.Status,
            PlayersInRoom: entity.PlayersInRoom,
            Players: playersResponse);
    }

    public static IEnumerable<RoomResponse> MapManyRoomsToManyRoomsResponse(ICollection<Room> rooms)
    {
        var roomsResponse = new List<RoomResponse>(rooms.Count);
        foreach (var room in rooms)
        {
            var playersResponse = new List<PlayerResponse>(room.Players.Count);
            foreach (var p in room.Players)
            {
                var pResponse = PlayerMapper.MapPlayerToPlayerResponse(p);
                playersResponse.Add(pResponse);
            }

            var roomResponse = RoomMapper.MapRoomToRoomResponse(room, playersResponse);
            roomsResponse.Add(roomResponse);
        }

        return roomsResponse;
    }

    public static RoomInLobbyResponse MapRoomToRoomInLobbyResponse(Room entity)
    {
        return new RoomInLobbyResponse(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid,
            ImgUrl: entity.AvatarUrl,
            Status: entity.Status,
            PlayersInRoom: entity.PlayersInRoom);
    }
}