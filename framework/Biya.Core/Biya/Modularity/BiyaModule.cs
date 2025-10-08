namespace Biya.Modularity;

public abstract class BiyaModule
{
    public virtual void PreConfigureServices(ServiceConfigurationContext context) { }
    public virtual void ConfigureServices(ServiceConfigurationContext context) { }
    public virtual void PostConfigureServices(ServiceConfigurationContext context) { }

    public virtual void OnApplicationInitialization(IServiceProvider provider) { }
    public virtual void OnApplicationShutdown(IServiceProvider provider) { }
}