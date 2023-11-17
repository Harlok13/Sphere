namespace Sphere.DAL.Models;

public class UserStatisticModel : BaseModel  
{
    public int Id { get; init; }

    public int Matches { get; set; }

    public int Loses { get; set; }

    public int Wins { get; set; }

    public int Draws { get; set; }

    public int Exp { get; set; }

    public int Money { get; set; }

    public int Likes { get; set; }

    public int Level { get; set; }

    public int Has21 { get; set; }

    public UserStatisticModel(int id) => Id = id;
}