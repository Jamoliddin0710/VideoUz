using System.Security.Claims;
using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace UI.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly IAccountRefitClient _accountRefitClient;
    private readonly IHttpContextAccessor accessor;
    public AccountController(IAccountRefitClient accountRefitClient, IHttpContextAccessor accessor)
    {
        _accountRefitClient = accountRefitClient;
        this.accessor = accessor;
    }

    // GET
    [HttpGet]
    public async Task<IActionResult> Login(string returnUrl = null)
    {
        var login = new LoginDTO();
        
        return View(login);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginDTO login)
    {
        if (!ModelState.IsValid)
        {
            return View(login);
        }

        if (HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }
        
        var response = await _accountRefitClient.Login(login);
        
        if (string.IsNullOrWhiteSpace(response.Data?.Token))
            return Redirect("/");
        
        var info = AuthHelper.JwtToModel(response?.Data?.Token);
        
        return await Auth(info, response.Data.Token);
    }
    
    [HttpGet]
    public async Task<IActionResult> Register()
    {
        var registerDto = new RegisterDTO();
        return View(registerDto);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterDTO registerDto)
    {
        if (!ModelState.IsValid)
        {
            return View(registerDto);
        }
        
        if (HttpContext?.User?.Identity?.IsAuthenticated ?? false)
        {
            return Redirect("/");
        }
        
        var response = await _accountRefitClient.Register(registerDto);
        
        if (string.IsNullOrWhiteSpace(response.Data?.Token))
            return Redirect("/");
        
        var info = AuthHelper.JwtToModel(response?.Data?.Token);
        
        return await Auth(info, response.Data.Token);
        
    }
    private async Task<ActionResult> Auth(CallbackResponseModel model, string token)
    {
        var identity = new ClaimsIdentity(
            model.Principal.Identity.Claims
                .Select(c => new Claim(c.Key, c.Value))
                .ToArray(),
            model.Principal.Identity.AuthenticationScheme);

        var principal = new ClaimsPrincipal(identity);

        var authProp = new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = model.Properties.AllowRefresh,
            IssuedUtc = model.Properties.IssuedUtc,
            ExpiresUtc = model.Properties.ExpiresUtc
        };

        if (!string.IsNullOrWhiteSpace(token))
            Response.Cookies.Append("token", token, new CookieOptions()
            {
                Expires = model.Properties.ExpiresUtc,
                Path = "/"
            });

        await HttpContext!.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, authProp);

        return Redirect("/");
    }
    public async Task<IActionResult> Logout()
    {
      await HttpContext!.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> AccessDenied()
    {
        return View();
    }
}