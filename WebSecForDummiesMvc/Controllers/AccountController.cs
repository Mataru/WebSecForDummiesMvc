using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebSecForDummiesMvc.Services;

namespace WebSecForDummiesMvc.Controllers;

[Authorize]
public class AccountController : Controller
{
    private readonly IDataService _dataService;

    public AccountController(IDataService dataService)
    {
        _dataService = dataService;
    }

    public async Task<IActionResult> Manage(string alert, string message)
    {
        ViewBag.Alert = alert;
        ViewBag.Message = message;

        var account = await _dataService.GetAccountFor(GetLoggedInUserName());
        return View(account);
    }

    [HttpPost]
    public async Task<IActionResult> ChangePassword([MinLength(4)]string newPassword)
    {
        if (!ModelState.IsValid)
        {
            var messages = ModelState
                .Where(p => p.Value != null)
                .SelectMany(x => x.Value!.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return RedirectToAction(nameof(Manage), new { alert = string.Join(Environment.NewLine, messages) });
        }

        var user = await _dataService.GetUser(GetLoggedInUserName());
        if (user != null)
        {
            user.Password = newPassword;
            await _dataService.UpdateUser(user);
        }

        return RedirectToAction(nameof(Manage), new { message = "Password changed successfully" });
    }

    public async Task<IActionResult> TransferFunds([Required] string receiver, decimal amount)
    {
        var account = await _dataService.GetAccountFor(GetLoggedInUserName());
        if (account is null) 
            return View("Error");
        if (account.Balance < amount)
            return RedirectToAction(nameof(Manage), new { alert = "You can't transfer more than your current balance" });

        account.Balance -= amount;
        await _dataService.UpdateAccount(account);

        return RedirectToAction(nameof(Manage), new { Message = "Transfer successful" });
    }

    private string GetLoggedInUserName()
    {
        return HttpContext.User.Identity?.Name ?? throw new Exception("Couldn't identify logged in user.");
    }
}