using App.Contracts.Data;
using App.Domain.Entities.RoomEntity;

namespace App.Contracts.Mapper;

public class RoomMapper
{
    public static IEnumerable<RoomInLobbyDto> MapManyRoomsToManyRoomsInLobbyDto(ICollection<Room> rooms)
    {
        var roomsInLobbyDto = new List<RoomInLobbyDto>(rooms.Count);
        foreach (var room in rooms)
        {
            var playersDto = new List<PlayerDto>(room.Players.Count);
            foreach (var p in room.Players)
            {
                var pResponse = PlayerMapper.MapPlayerToPlayerDto(p);
                playersDto.Add(pResponse);
            }

            var roomDto = MapRoomToRoomInLobbyDto(room);
            roomsInLobbyDto.Add(roomDto);
        }

        return roomsInLobbyDto;
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

    public static InitRoomDataDto MapRoomToInitRoomDataDto(Room entity)
    {
        return new InitRoomDataDto(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid);
    }
    
    public static IEnumerable<RoomDto> MapManyRoomsToManyRoomsDto(ICollection<Room> rooms)
    {
        var roomsDto = new List<RoomDto>(rooms.Count);
        foreach (var room in rooms)
        {
            var playersDto = new List<PlayerDto>(room.Players.Count);
            foreach (var p in room.Players)
            {
                var playerDto = PlayerMapper.MapPlayerToPlayerDto(p);
                playersDto.Add(playerDto);
            }

            var roomDto = MapRoomToRoomDto(room, playersDto);
            roomsDto.Add(roomDto);
        }

        return roomsDto;
    }

    public static RoomDto MapRoomToRoomDto(Room entity, IEnumerable<PlayerDto> playersResponse)
    {
        return new RoomDto(
            Id: entity.Id,
            RoomName: entity.RoomName,
            RoomSize: entity.RoomSize,
            StartBid: entity.StartBid,
            MinBid: entity.MinBid,
            MaxBid: entity.MaxBid,
            AvatarUrl: entity.AvatarUrl,
            PlayersInRoom: entity.PlayersInRoom,
            Status: entity.Status,
            Bank: entity.Bank,
            LowerStartMoneyBound: entity.LowerStartMoneyBound,
            UpperStartMoneyBound: entity.UpperStartMoneyBound,
            Players: playersResponse);
    }
}