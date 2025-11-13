using Biya.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Biya.EntityFrameworkCore;

public class BiyaDbContextFactory<TDbContext> : IDbContextFactory<TDbContext>, ITransientDependency
    where TDbContext : DbContext
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IInjectPropertiesService _injectPropertiesService;

    public BiyaDbContextFactory(
        IServiceScopeFactory serviceScopeFactory,
        IInjectPropertiesService injectPropertiesService
    )
    {
        _serviceScopeFactory = serviceScopeFactory;
        _injectPropertiesService = injectPropertiesService;
    }

    public TDbContext CreateDbContext()
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
            _injectPropertiesService.InjectProperties(context);

            return context;
        }
    }
}