using Application.DTOs;
using Application.Models;
using Refit;

namespace Application.ServiceContract;

public interface IChannelRefitService
{
    [Post("/channel/create")]
    Task Create([Body] CreateOrUpdateChannelDTO createOrUpdateChannelDto);
    [Get("/channel/getuserchannels")]
    Task <ServiceResponse<FilterResponseModel<ChannelDTO>>> GetUserChannels();
}