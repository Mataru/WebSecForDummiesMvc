using Microsoft.EntityFrameworkCore;
using MyCryptoWallet.Data;
using MyCryptoWallet.Models;

namespace MyCryptoWallet.Services;

public interface IDataService
{
    Task<User?> GetUser(string username);
    Task<Account?> GetAccountFor(string username);
    Task CreateAccountFor(User user);
    Task UpdateAccount(Account account);
    Task UpdateUser(User user);
}
public class DataService : IDataService
{
    private readonly WalletContext _context;

    public DataService(WalletContext context)
    {
        _context = context;
    }
    
    public Task<User?> GetUser(string username)
    {
        return _context.Users.FirstOrDefaultAsync(user => user.Username == username);
    }

    public Task<Account?> GetAccountFor(string username)
    {
        return _context.Accounts.FirstOrDefaultAsync(account => account.Owner.Username == username);
    }

    public Task CreateAccountFor(User user)
    {
        _context.Users.Add(user);
        _context.Accounts.Add(new Account { Owner = user, Balance = new Random().Next(0, 10_000), Currency = "BTC" });
        
        return _context.SaveChangesAsync();
    }

    public Task UpdateAccount(Account account)
    {
        _context.Update(account);

        return _context.SaveChangesAsync();
    }

    public Task UpdateUser(User user)
    {
        _context.Update(user);

        return _context.SaveChangesAsync();
    }
}