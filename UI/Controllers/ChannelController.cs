using Application.DTOs;
using Application.ServiceContract;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        var savedmodel  = HttpContext.Session.GetString("channelmodel");
        if (!string.IsNullOrWhiteSpace(savedmodel))
        {
            var channel = JsonConvert.DeserializeObject<Channel>(savedmodel);
            model.Name  = channel?.Name ?? string.Empty;
            model.Description  = channel?.Description ?? string.Empty;
        }
        var errorString = HttpContext.Session.GetString("errors");
        if (!string.IsNullOrWhiteSpace(errorString))
        {
          var errors =  JsonConvert.DeserializeObject<List<ErrorModel>>(errorString);
            foreach (var variableError in errors)
            {
                ModelState.AddModelError(variableError.Code, variableError.Message);
            }
        }
      
       
        var userchannels = await _channelRefitService.GetUserChannels();
        /// var userchannels = User.GetUserId();
        if (userchannels?.Data?.Data?.Any() ?? false)
        {
            var first = userchannels.Data.Data.FirstOrDefault();
            model.Id = first.Id;
            model.Name = first.Name;
            model.Description = first.Description; 
        }

        return View(model);
    }

    public async Task<IActionResult> CreateOrEditChannel(CreateOrUpdateChannelDTO model)
    {
        if (!ModelState.IsValid)
        {
            var errors = new List<ErrorModel>();
            
            foreach (var error in ModelState)
            {
                if (error.Value?.Errors?.FirstOrDefault()?.ErrorMessage == null)
                  continue;
                
                errors.Add(new ErrorModel()
                {
                    Code = error.Key,
                    Message = error.Value.Errors.FirstOrDefault()?.ErrorMessage,
                });
            }
            
            HttpContext.Session.SetString("errors", JsonConvert.SerializeObject(errors));
            HttpContext.Session.SetString("channelmodel", JsonConvert.SerializeObject(model));
            return RedirectToAction("Index");
        }
        try
        {
            await _channelRefitService.Create(model);
            HttpContext.Session.SetString("ChannelModelFromSession", JsonConvert.SerializeObject(model));
        }
        catch (Exception exception)
        {
          Console.WriteLine(exception.Message);
        }
        
        return RedirectToAction("Index");
    }
}