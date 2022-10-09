using System.Diagnostics;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyCryptoWallet.Models;
using MyCryptoWallet.Services;

namespace MyCryptoWallet.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IDataService _dataService;

    public HomeController(ILogger<HomeController> logger, IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(User user)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Message = "Come on, don't be daft. Fill in both fields.";
            return View();
        }

        var registeredUser = await _dataService.GetUser(user.Username);
        if (registeredUser is null || registeredUser.Password != user.Password)
        {
            ViewBag.Message = "Couldn't log you in :)";
            return View();
        }

        await SignIn(user.Username);

        return RedirectToAction("Manage", "Account");
    }

    [HttpPost]
    public async  Task<IActionResult> Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Message = "Come on, don't be daft. Fill in both fields.";
            return View();
        }

        var registeredUser = await _dataService.GetUser(newUser.Username);
        if (registeredUser is not null)
        {
            ViewBag.Message = "Sorry bro, that username is already in use :(";
            return View();
        }

        await _dataService.CreateAccountFor(newUser);

        await SignIn(newUser.Username);

        return RedirectToAction(nameof(Index));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();

        return RedirectToAction(nameof(Index));
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    private async Task SignIn(string username)
    {
        var claims = new List<Claim>()
        {
            new(ClaimTypes.Name, username)
        };
        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        var properties = new AuthenticationProperties
        {
            AllowRefresh = true,
            IsPersistent = true,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), properties);
    }
}