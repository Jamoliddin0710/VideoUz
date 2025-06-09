using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Refit;
using UI.Services;

namespace UI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OllamaController(IOllamaRefitService _ollamaService, ILogger<OllamaController> _logger) : ControllerBase
{
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] PromptRequest request)
    {
        try
        {
            _logger.LogInformation($"Processing chat request with model: {request.Model}");

            var ollamaRequest = new OllamaRequest
            {
                Model = request.Model ?? "llama2:latest",
                Messages = new List<Message>
                {
                    new Message { Role = "user", Content = request.Prompt }
                }
            };

            var response = await _ollamaService.ChatAsync(ollamaRequest);
            return Ok(new
            {
                success = true,
                model = response.Message,
                response = response.Message.Content
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat request");
            return StatusCode(500, new
            {
                success = false,
                error = ex.Message,
                details = ex.InnerException?.Message
            });
        }
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModels()
    {
        try
        {
            var response = await _ollamaService.GetModelsAsync();
            return Ok(response.Models);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting models");
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("pull")]
    public async Task<IActionResult> PullModel([FromBody] PullModelRequest request)
    {
        try
        {
            await _ollamaService.PullModelAsync(new Dictionary<string, string>
            {
                { "name", request.ModelName }
            });
            return Ok(new { success = true, message = $"Model {request.ModelName} pull initiated" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error pulling model {request.ModelName}");
            return StatusCode(500, new { error = ex.Message });
        }
    }
}

public class PullModelRequest
{
    public string ModelName { get; set; }
}