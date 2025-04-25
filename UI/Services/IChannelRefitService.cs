using Application.DTOs;
using Application.Models;
using Refit;

namespace UI.Services;

public interface IChannelRefitService
{
    [Post("/channel/create")]
    Task Create([Body] CreateOrUpdateChannelDTO createOrUpdateChannelDto);
    [Get("/channel/getuserchannels")]
    Task <ServiceResponse<FilterResponseModel<ChannelDTO>>> GetUserChannels();  
    [Get("/channel/getuserchannels")]
    Task <ServiceResponse<bool>> UsersChannelExists();
}