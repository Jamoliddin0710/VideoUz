using Application.Models;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace UI.Services;

public interface IOllamaRefitService
{
    [Post("/api/chat")]
    Task<OllamaResponse> ChatAsync([Body] OllamaRequest request);

    [Get("/api/tags")]
    Task<ModelsResponse> GetModelsAsync();

    [Post("/api/pull")]
    Task PullModelAsync([Body] Dictionary<string, string> request);
}