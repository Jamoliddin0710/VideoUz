using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UI.Services;

namespace UI.Controllers;

public class ChannelController : Controller
{
    private readonly IChannelRefitService _channelRefitService;

    public ChannelController(IChannelRefitService channelRefitService)
    {
        _channelRefitService = channelRefitService;
    }

    [Authorize(Roles = nameof(Role.Admin))]
    public async Task<IActionResult> Index()
    {
        var model = new CreateOrUpdateChannelDTO();
        var userchannels = await _channelRefitService.GetUserChannels();
        /// var userchannels = User.GetUserId();

        return View(model);
    }

    public async Task<IActionResult> CreateChannel(CreateOrUpdateChannelDTO model)
    {
        await _channelRefitService.Create(model);
        return RedirectToAction("Index");
    }
}