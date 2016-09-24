using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Orchard.DisplayManagement.ModelBinding;
using Orchard.Environment;
using Orchard.Environment.Commands;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Folders;
using Orchard.Environment.Shell;
using Orchard.FileSystem;
using Orchard.Hosting;
using Orchard.Hosting.FileSystem;
using Orchard.Hosting.Mvc.Filters;
using Orchard.Hosting.Mvc.ModelBinding;
using Orchard.Hosting.Mvc.Razor;
using Orchard.Hosting.Routing;
using Orchard.Hosting.Web.Mvc.ModelBinding;

namespace Microsoft.AspNetCore.Mvc.Modules.Hosting
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModuleServices(this IServiceCollection services)
        {
            return AddModuleServices(services, "Modules", null);
        }

        public static IServiceCollection AddModuleServices(this IServiceCollection services, string modulesPath)
        {
            return AddModuleServices(services, modulesPath, null);
        }

        public static IServiceCollection AddModuleServices(this IServiceCollection services, string modulesPath, Action<MvcOptions> setupAction)
        {
            services.AddWebHost();
            services.AddModuleFolder(modulesPath);

            services
                .AddMvcCore(options =>
                {
                    options.Filters.Add(new ModelBinderAccessorFilter());
                    options.Filters.Add(typeof(AutoValidateAntiforgeryTokenAuthorizationFilter));
                    options.ModelBinderProviders.Insert(0, new CheckMarkModelBinderProvider());

                    setupAction?.Invoke(options);
                })
                .AddViews()
                .AddViewLocalization()
                .AddRazorViewEngine()
                .AddJsonFormatters();

            services.AddScoped<IModelUpdaterAccessor, LocalModelBinderAccessor>();
            services.AddTransient<IFilterProvider, DependencyFilterProvider>();
            services.AddTransient<IApplicationModelProvider, ModuleAreaRouteConstraintApplicationModelProvider>();

            services.Configure<RazorViewEngineOptions>(configureOptions: options =>
            {
                var expander = new ModuleViewLocationExpander();
                options.ViewLocationExpanders.Add(expander);

                var extensionLibraryService = services.BuildServiceProvider().GetService<IExtensionLibraryService>();
                ((List<MetadataReference>)options.AdditionalCompilationReferences).AddRange(extensionLibraryService.MetadataReferences());
            });

            // Register the list of services to be resolved later on,
            // hence AddOrchardMvc should be the last method called
            services.AddSingleton(_ => services);

            return services;
        }

        public static IServiceCollection AddWebHost(this IServiceCollection services)
        {
            return services.AddHost(internalServices =>
            {
                internalServices.AddLogging();
                internalServices.AddOptions();
                internalServices.AddLocalization();
                internalServices.AddHostCore();
                internalServices.AddExtensionManager();
                internalServices.AddCommands();

                internalServices.AddSingleton<IHostEnvironment, WebHostEnvironment>();
                internalServices.AddSingleton<IOrchardFileSystem, HostedFileSystem>();
                internalServices.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            });
        }


        public static IServiceCollection AddOrchard(this IServiceCollection services)
        {
            services.AddWebHost();

            services.AddMultiTenancy("Sites");

            services.AddModuleServices();
            services.AddModuleFolder("Modules");

            // Save the list of service definitions
            services.AddSingleton(_ => services);

            return services;
        }

        public static IApplicationBuilder UserOrchard(this IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseModules(env, loggerFactory);

            return app;
        }
    }
}