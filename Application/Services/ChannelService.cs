using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Domain.RepositoryContracts;
using Mapster;

namespace Application.Services;

public class ChannelService : IChannelService
{
    private readonly IUnitOfWork _unitOfWork;

    public ChannelService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task CreateAsync(CreateOrUpdateChannelDTO channelDto , long userId)
    {
        if (await _unitOfWork.ChannelRepo.AnyAsync(a=>a.Name.ToLower().Equals(channelDto.Name.ToLower())))
        {
            throw new Exception($"Channel {channelDto.Name} already exists");
        }
        var channel = channelDto.Adapt<Channel>();
        channel.AppUserId = userId;
        _unitOfWork.ChannelRepo.Add(channel);
        try
        {
            await _unitOfWork.CompleteAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
       
    }

    public async Task<ChannelDTO?> GetByIdAsync(long Id)
    {
        var channel = await _unitOfWork.ChannelRepo.GetByIdAsync(Id);
        if (channel != null)
        {
            return channel.Adapt<ChannelDTO>();
        }

        return null;
    }

    public async Task<FilterResponseModel<ChannelDTO>> GetAllAsync()
    {
        var channels = await _unitOfWork.ChannelRepo.GetAllAsync();
        if (channels?.Any() ?? false)
        {
           var result = channels.Adapt<List<ChannelDTO>>();
           return new FilterResponseModel<ChannelDTO>()
           {
               ItemsCount = result.Count,
               Data = result.Adapt<List<ChannelDTO>>()
           };
        }

        return new FilterResponseModel<ChannelDTO>();
    }

    public async Task UpdateAsync(long Id, CreateOrUpdateChannelDTO createOrUpdateChannelDto)
    {
        var channel = await _unitOfWork.ChannelRepo.GetByIdAsync(Id);
        if (channel is not null)
        {
            var model = createOrUpdateChannelDto.Adapt<Channel>();
            _unitOfWork.ChannelRepo.Update(channel, model);
            await _unitOfWork.CompleteAsync();
        }
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var channel = await _unitOfWork.ChannelRepo.GetByIdAsync(id);
        if (channel is not null)
        {
            _unitOfWork.ChannelRepo.Remove(channel);
            return await _unitOfWork.CompleteAsync();
        }

        return false;
    }

    public async Task<FilterResponseModel<ChannelDTO>> GetUsersChannel(long userId)
    {
        var result = await _unitOfWork.ChannelRepo.GetAllAsync(a=>a.AppUserId == userId);
        if (result?.Any() ?? false)
        {
            return new FilterResponseModel<ChannelDTO>()
            {
                ItemsCount = result.Count(),
                Data = result.Adapt<List<ChannelDTO>>()
            }; 
        }

        return new FilterResponseModel<ChannelDTO>();
    }
}