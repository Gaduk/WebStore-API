namespace Domain.Entities;

public class Good
{
    public int      Id        { get; init; }
    public string?  Name      { get; set ; }
    public int      Price     { get; set ; }
    public string[] ImageUrls { get; set ; } = [];
}