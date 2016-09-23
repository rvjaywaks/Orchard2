using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orchard.Environment.Shell;
using Orchard.Environment.Shell.Builders;
using Orchard.Environment.Shell.Descriptor;
using Orchard.Environment.Shell.Descriptor.Settings;
using Orchard.FileSystem;
using Orchard.Hosting.Services;
using Orchard.Services;

namespace Orchard.Hosting
{
    public static class HostServiceExtensions
    {
        public static IServiceCollection AddHost(
            this IServiceCollection services, Action<IServiceCollection> additionalDependencies)
        {
            services.AddFileSystems();
            additionalDependencies(services);

            return services;
        }

        public static IServiceCollection AddHostCore(this IServiceCollection services)
        {
            services.AddSingleton<IClock, Clock>();

            services.AddSingleton<DefaultOrchardHost>();
            services.AddSingleton<IOrchardHost>(sp => sp.GetRequiredService<DefaultOrchardHost>());
            services.AddSingleton<IShellDescriptorManagerEventHandler>(sp => sp.GetRequiredService<DefaultOrchardHost>());
            {
                // Use a single default site by default, i.e. if AddMultiTenancy hasn't been called before
                services.TryAddSingleton<IShellSettingsManager, SingleShellSettingsManager>();

                // Use all existing modules
                services.TryAddSingleton<IShellDescriptorManager, SingleShellDescriptorManager>();

                services.AddSingleton<IShellContextFactory, ShellContextFactory>();
                {
                    services.AddSingleton<ICompositionStrategy, CompositionStrategy>();

                    services.AddSingleton<IShellContainerFactory, ShellContainerFactory>();
                }
            }
            services.AddSingleton<IRunningShellTable, RunningShellTable>();

            return services;
        }
    }
}