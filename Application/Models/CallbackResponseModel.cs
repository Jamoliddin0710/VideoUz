namespace Application.Models;

public class CallbackResponseModel
{
    public CallbackPrincipalResponseModel Principal { get; set; }

    public CallbackPropertiesResponseModel Properties { get; set; }
}

public class CallbackPropertiesResponseModel
{
    public bool IsPersistent { get; set; }

    public bool AllowRefresh { get; set; }

    public DateTimeOffset? IssuedUtc { get; set; }

    public DateTimeOffset? ExpiresUtc { get; set; }
}

public class CallbackPrincipalResponseModel
{
    public CallbackIdentityResponseModel Identity { get; set; }
}

public class CallbackIdentityResponseModel
{
    public Dictionary<string, string> Claims { get; set; }

    public string AuthenticationScheme { get; set; }
}