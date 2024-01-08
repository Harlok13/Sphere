// using App.Domain.Entities.RoomEntity;
//
// namespace App.Application.Repositories.RoomRepository;
//
// public abstract class RoomRepositoryDecorator : IRoomRepository
// {
//     protected readonly IRoomRepository RoomRepository;
//
//     protected RoomRepositoryDecorator(IRoomRepository roomRepository)
//     {
//         RoomRepository = roomRepository;
//     }
//
//     public abstract Task RemoveAsync(Guid roomId, CancellationToken cT);
//
//     public virtual async Task AddAsync(Room room, CancellationToken cT)
//     {
//         await RoomRepository.AddAsync(room, cT);
//     }
//
//     public virtual async Task<Room?> GetByIdAsync(Guid roomId, CancellationToken cT)
//     {
//         return await RoomRepository.GetByIdAsync(roomId, cT);
//     }
//
//     public virtual async Task<Room?> GetRoomByIdAsNoTrackingAsync(Guid roomId, CancellationToken cT)
//     {
//         return await RoomRepository.GetByIdAsNoTrackingAsync(roomId, cT);
//     }
//
//     public virtual async Task<ICollection<Room>?> GetFirstPageAsNoTrackingAsync(CancellationToken cT)
//     {
//         return await RoomRepository.GetFirstPageAsNoTrackingAsync(cT);
//     }
// }