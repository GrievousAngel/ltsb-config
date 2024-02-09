using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ConfigAdmin;

public static class ServiceRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services.TryAddTransient<IConfigService, ConfigService>();

        services.Configure<AppSettings>(o => config.GetSection(AppSettings.SectionKey).Bind(o));

        return services;
    }
}