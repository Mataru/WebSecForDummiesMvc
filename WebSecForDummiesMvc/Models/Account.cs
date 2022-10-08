namespace WebSecForDummiesMvc.Models;

public class Account
{
    public int Id { get; set; }
    public User Owner { get; set; }
    public decimal Balance { get; set; }
    public string Currency { get; set; }
}