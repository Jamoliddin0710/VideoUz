
using Domain.Models;
using Refit;

namespace Infrastructure.Client;

public interface IGPTRefitService
{
    [Post("/v1/chat/completions")]
    Task<GPTResponse> SendMessage(OpenAiChatRequest request);
}