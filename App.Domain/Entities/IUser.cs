namespace App.Domain.Entities;

public interface IUser
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
}