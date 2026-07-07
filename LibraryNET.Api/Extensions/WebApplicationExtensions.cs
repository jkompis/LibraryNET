using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryNET.Api.Extensions;

public interface IApiDefinition
{
    /// <summary>
    /// Endpoint registration for API definition
    /// </summary>
    /// <param name="app">web application</param>
    /// <returns></returns>
    void RegisterEndpoints(WebApplication app);
    
    /// <summary>
    /// Service registration for API definition
    /// </summary>
    /// <param name="serviceCollection">service collection</param>
    /// <returns></returns>
    void RegisterServices(IServiceCollection serviceCollection);
}

public static class WebApplicationExtensions
{
    // ReSharper disable once UnusedMethodReturnValue.Global
    public static WebApplication SetupWebApplication(this WebApplication self)
    {
#if DEBUG
        // debug exceptions
        self.UseDeveloperExceptionPage();
#endif

        self.UseRouting();

        var apiDefinitions = self.Services.GetRequiredService<IReadOnlyCollection<IApiDefinition>>();

        // register all IMinimalApi endpoints
        foreach (var apiDefinition in apiDefinitions)
        {
            apiDefinition.RegisterEndpoints(self);
        }

        return self;
    }

    // ReSharper disable once UnusedMethodReturnValue.Global
    public static IServiceCollection RegisterServices(this IServiceCollection self, params Assembly[] assemblies)
    {
        var minimalApis = new List<IApiDefinition>();

        // find IMinimalApi implementations in provided assemblies
        foreach (var assembly in assemblies)
        {
            minimalApis.AddRange(assembly.ExportedTypes
                .Where(x => typeof(IApiDefinition).IsAssignableFrom(x) && !x.IsInterface).Select(Activator.CreateInstance)
                .Cast<IApiDefinition>());
        }

        // register services necessary for IMinimalApi implementations
        foreach (var minimalApi in minimalApis)
        {
            minimalApi.RegisterServices(self);
        }

        // register IMinimalApi implementations as a collection
        return self.AddSingleton(minimalApis as IReadOnlyCollection<IApiDefinition>);
    }
}