using App.Domain.Entities;
using App.Domain.Entities.RoomEntity;

namespace App.Application.Services.Interfaces;

public interface IGame21Service
{
    void StartGame(Room room);
}