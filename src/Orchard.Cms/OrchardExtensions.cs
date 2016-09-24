using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Modules.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orchard.BackgroundTasks;
using Orchard.Data;
using Orchard.DeferredTasks;
using Orchard.DisplayManagement;
using Orchard.Environment.Extensions.Folders;
using Orchard.Environment.Shell;

namespace Orchard.Cms
{
    public static class OrchardExtensions
    {
        public static IServiceCollection AddOrchardCms(this IServiceCollection services)
        {
            services.AddTheming();
            services.AddThemeFolder("Themes");
            services.AddMultiTenancy("Sites");

            services.AddDeferredTasks();
            services.AddDataAccess();
            services.AddBackgroundTasks();

            services.AddModuleServices();

            return services;
        }

        public static IApplicationBuilder UseOrchardCms(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseModules(env, loggerFactory);

            return app;
        }
    }
}