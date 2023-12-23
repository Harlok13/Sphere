using App.Contracts.Data;
using App.Contracts.Requests;
using App.Contracts.Responses;
using App.Domain.Entities.RoomEntity;

namespace App.Contracts.Mapper;

public class RoomMapper
{
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

            var roomResponse = MapRoomToRoomResponse(room, playersResponse);
            roomsResponse.Add(roomResponse);
        }

        return roomsResponse;
    }

    public static RoomInLobbyDto MapRoomToRoomInLobbyDto(Room entity)
    {
        return new RoomInLobbyDto(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MaxBid: entity.MaxBid,
            MinBid: entity.MinBid,
            AvatarUrl: entity.AvatarUrl,
            Status: entity.Status,
            PlayersInRoom: entity.PlayersInRoom,
            Bank: entity.Bank,
            LowerStartMoneyBound: entity.LowerStartMoneyBound,
            UpperStartMoneyBound: entity.UpperStartMoneyBound);
    }

    public static InitRoomDataResponse MapRoomToInitRoomDataResponse(Room entity)
    {
        return new InitRoomDataResponse(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid);
    }

    public static RoomDto MapRoomRequestToRoomDto(RoomRequest request)
    {
        return new RoomDto(
            RoomName: request.RoomName,
            RoomSize: request.RoomSize,
            StartBid: request.StartBid,
            MinBid: request.MinBid,
            MaxBid: request.MaxBid,
            AvatarUrl: request.AvatarUrl,
            Status: request.Status,
            PlayersInRoom: request.PlayersInRoom);
    }
    
    private static RoomResponse MapRoomToRoomResponse(Room entity, IEnumerable<PlayerResponse> playersResponse)
    {
        return new RoomResponse(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid,
            AvatarUrl: entity.AvatarUrl,
            Status: entity.Status,
            PlayersInRoom: entity.PlayersInRoom,
            Bank: entity.Bank,
            Players: playersResponse);
    }
}