using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;

public class ChannelController : Controller
{
    [Route("/channel")]
    [Authorize(Roles = nameof(Role.Admin))]
    public IActionResult Index()
    {
        return View();
    }

}