using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ChannelController : BaseApiController
{
    private readonly IChannelService _channelService;

    public ChannelController(IChannelService channelService)
    {
        _channelService = channelService;
    }

    [HttpPost]
  
    public async Task<IActionResult> Create(CreateOrUpdateChannelDTO createOrUpdateChannelDto)
    {
        HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        await _channelService.CreateAsync(createOrUpdateChannelDto);
        return NoContent();
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<ChannelDTO>>>> GetAllAsync()
    {
        return Ok(await _channelService.GetAllAsync());
    }

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<ChannelDTO>>> GetById(long Id)
    {
        return Ok(await _channelService.GetByIdAsync(Id));
    }
    
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ServiceResponse<FilterResponseModel<ChannelDTO>>>> GetUserChannels()
    {
        var userId = User.GetUserId();
        if (userId.HasValue || userId == 0)
        {
            return Ok(await _channelService.GetUsersChannel(userId.Value));
        }

        return new ServiceResponse<FilterResponseModel<ChannelDTO>>()
        {
            Error = new ErrorModel("401", "User not authorized"),
            IsSuccessful = false,
            Data = null,
        };
    }
    

    [HttpPut]
    public async Task<IActionResult> Update(long Id, CreateOrUpdateChannelDTO categoryDto)
    {
        await _channelService.UpdateAsync(Id, categoryDto);
        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(long Id)
    {
        await _channelService.DeleteAsync(Id);
        return NoContent();
    }
}