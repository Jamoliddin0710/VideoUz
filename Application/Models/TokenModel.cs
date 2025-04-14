namespace Application.Models;

public class TokenModel : RefreshTokenModel
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}

