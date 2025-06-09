using Application.DTOs;
using Application.Helpers;
using Application.Models;
using Application.ServiceContract;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class ChannelController(IChannelService _channelService) : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrUpdateChannelDTO createOrUpdateChannelDto)
    {
        var userId = User.GetUserId();
        if (userId.HasValue || userId == 0)
        {
            await _channelService.CreateAsync(createOrUpdateChannelDto, userId.Value);
            return NoContent();
        }

        return Unauthorized("User  not authorized");
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

    [HttpGet]
    public async Task<ActionResult<ServiceResponse<bool>>> UsersChannelExists()
    {
        var userId = User.GetUserId();
        if (userId.HasValue)
        {
            var result = await _channelService.UsersChannelExists(userId.Value);
            return Ok(result);
        }

        return Ok(false);
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