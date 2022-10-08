using System.ComponentModel.DataAnnotations;

namespace WebSecForDummiesMvc.Models;

public class User
{
    public int Id { get; set; }

    [Required]
    [MinLength(4)]
    public string Username { get; set; }

    [Required]
    [MinLength(4)]
    public string Password { get; set; }

    public DateTimeOffset RegisteredTimestamp { get; set; }
}