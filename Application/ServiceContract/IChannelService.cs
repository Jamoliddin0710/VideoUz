using Application.DTOs;

namespace Application.ServiceContract;

public interface IChannelService
{
    // Create a new channel
    Task CreateAsync(CreateOrUpdateChannelDTO orUpdateChannel , long userId);

    // Get a channel by its ID
    Task<ChannelDTO> GetByIdAsync(long Id);

    // Get all channels
    Task<FilterResponseModel<ChannelDTO>> GetAllAsync();

    // Update an existing channel
    Task UpdateAsync(long Id, CreateOrUpdateChannelDTO createOrUpdateChannelDto);

    // Delete a channel by its ID
    Task<bool> DeleteAsync(long id);

    Task<FilterResponseModel<ChannelDTO>> GetUsersChannel(long userId);
}
