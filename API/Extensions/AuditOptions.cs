namespace API.Extensions;

public class AuditOptions(IServiceCollection services)
{
    public IServiceCollection Services { get; } = services;
}