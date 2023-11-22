using System.ComponentModel.DataAnnotations;

namespace Sphere.DAL.Models;

public class LobbyModel
{
    public int Id { get; set; }
    
    public int Size { get; set; }
    
    public string Name { get; set; } = null!;
    
    public int StartBid { get; set; }
    
    public int Bid { get; set; }
    
    public string ImgUrl { get; set; } = null!;
}