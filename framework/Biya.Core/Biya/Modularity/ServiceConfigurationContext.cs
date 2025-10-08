using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Biya.Modularity;

public class ServiceConfigurationContext
{
    public IServiceCollection Services { get; }
    public IConfiguration Configuration { get; }

    public ServiceConfigurationContext(IServiceCollection services, IConfiguration configuration)
    {
        Services = services;
        Configuration = configuration;
    }
}