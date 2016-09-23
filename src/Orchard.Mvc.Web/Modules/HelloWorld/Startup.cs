﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Orchard;

namespace HelloWorld
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute
            (
                name: "Home",
                area: "HelloWorld",
                template: "",
                controller: "Home",
                action: "Index"
            );
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {

        }
    }
}
