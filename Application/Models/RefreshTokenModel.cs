namespace Application.Models;

public class RefreshTokenModel
{
    public string RefreshToken { get; set; }

    public DateTime RefreshTokenExpiration { get; set; }
}