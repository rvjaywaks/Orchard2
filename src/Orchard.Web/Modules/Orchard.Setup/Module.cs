﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Orchard.Hosting;
using Orchard.Setup.Services;

namespace Orchard.Setup
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<ISetupService, SetupService>();

            new ShellModule()
                .ConfigureServices(serviceCollection);
        }

        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute(
                name: "Setup",
                area: "Orchard.Setup",
                template: "",
                controller: "Setup",
                action: "Index"
            );
        }
    }
}
