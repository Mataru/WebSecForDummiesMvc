using System.ComponentModel.DataAnnotations;

namespace SqlInjectionDemo.Models;

public class Post
{
    public int Id { get; set; }
    [Required]
    public string Title { get; set; }
    [MinLength(4)]
    public string Text { get; set; }
    public DateTimeOffset CreatedTimestamp { get; set; }
}