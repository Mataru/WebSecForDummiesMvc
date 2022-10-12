namespace SqlInjectionDemo.Models;

public class Board
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Post> Posts { get; set; } = new();
}