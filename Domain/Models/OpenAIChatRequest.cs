namespace Domain.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class OpenAiChatRequest
{
    [JsonPropertyName("model")] public string Model { get; set; } = "gpt-4o-mini";

    [JsonPropertyName("messages")] public List<OpenAiMessage> Messages { get; set; }

    [JsonPropertyName("temperature")] public double? Temperature { get; set; } = 1;

    [JsonPropertyName("top_p")] public double? TopP { get; set; }

    [JsonPropertyName("n")] public int? N { get; set; }

    [JsonPropertyName("stream")] public bool? Stream { get; set; }

    [JsonPropertyName("stop")] public List<string>? Stop { get; set; }

    [JsonPropertyName("max_tokens")] public int? MaxTokens { get; set; } = null;

    [JsonPropertyName("presence_penalty")] public double? PresencePenalty { get; set; }

    [JsonPropertyName("frequency_penalty")]
    public double? FrequencyPenalty { get; set; }

    [JsonPropertyName("user")] public string? User { get; set; }
}

public class OpenAiMessage
{
    [JsonPropertyName("role")] public string Role { get; set; } // "system", "user", "assistant"

    [JsonPropertyName("content")] public string Content { get; set; }
}