using Application.DTOs;
using Application.Helpers;
using Application.ServiceContract;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers;

public class VideoController : Controller
{
    private readonly IChannelRefitService _channelRefitService;

    public VideoController(IChannelRefitService channelRefitService)
    {
        _channelRefitService = channelRefitService;
    }

    public async Task<IActionResult> CreateEditVideo(long Id)
    {
        var userId = User.GetUserId();
        if (userId.HasValue && (await _channelRefitService.UsersChannelExists()).Data)
        {
            TempData["notication"] = "false; Not found; No channel assosiated with your account was found";
            return RedirectToAction("Index", "Channel");
        }

        var createOrEditVideoDto = new CreateOrEditVideoDTO();
        return View(createOrEditVideoDto);
    }
}