namespace Biya.DependencyInjection;

public interface IInjectPropertiesService
{
    TService InjectProperties<TService>(TService instance) where TService : notnull;

    TService InjectUnsetProperties<TService>(TService instance) where TService : notnull;
}