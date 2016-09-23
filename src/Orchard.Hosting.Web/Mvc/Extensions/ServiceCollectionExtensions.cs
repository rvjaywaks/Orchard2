using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
using Microsoft.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Orchard.DisplayManagement.ModelBinding;
using Orchard.DisplayManagement.TagHelpers;
using Orchard.Environment.Extensions;
using Orchard.Environment.Extensions.Folders;
using Orchard.Hosting.Mvc.Filters;
using Orchard.Hosting.Mvc.ModelBinding;
using Orchard.Hosting.Mvc.Razor;
using Orchard.Hosting.Routing;
using Orchard.Hosting.Web.Mvc.ModelBinding;

namespace Orchard.Hosting.Mvc
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddOrchardMvc(this IServiceCollection services)
        {
            return AddOrchardMvc(services, "Modules", null);
        }

        public static IServiceCollection AddOrchardMvc(this IServiceCollection services, string modulesPath)
        {
            return AddOrchardMvc(services, modulesPath, null);
        }

        public static IServiceCollection AddOrchardMvc(this IServiceCollection services, string modulesPath, Action<MvcOptions> setupAction)
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

    }
}