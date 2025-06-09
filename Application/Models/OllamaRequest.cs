namespace Application.Models;

using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PromptRequest
{
    public string Prompt { get; set; }
    public string Model { get; set; }
}

public class Message
{
    [JsonPropertyName("role")] public string Role { get; set; }

    [JsonPropertyName("content")] public string Content { get; set; }
}

public class OllamaRequest
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("messages")] public List<Message> Messages { get; set; }

    [JsonPropertyName("stream")] public bool? Stream { get; set; } = false;
}

public class OllamaResponse
{
    [JsonPropertyName("model")] public string Model { get; set; }

    [JsonPropertyName("message")] public Message Message { get; set; }

    [JsonPropertyName("done")] public bool Done { get; set; }
}

public class ModelInfo
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("modified_at")] public string ModifiedAt { get; set; }

    [JsonPropertyName("size")] public long Size { get; set; }
}

public class ModelsResponse
{
    [JsonPropertyName("models")] public List<ModelInfo> Models { get; set; }
}