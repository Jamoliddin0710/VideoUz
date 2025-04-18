using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

[Authorize(Roles = nameof(Role.Admin))]
public class AdminController : Controller
{
    public async Task<IActionResult> Category()
    {
        return View();
    }
}