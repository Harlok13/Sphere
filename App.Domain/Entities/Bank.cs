using App.Domain.Entities.RoomEntity;

namespace App.Domain.Entities;

public class Bank
{
    public Guid Id { get; set; }
    public int TotalMoney { get; set; }
    // public ICollection<> SeparatedBank { get; set; }
    public Guid RoomId { get; set; }
    public Room Room { get; set; }
}