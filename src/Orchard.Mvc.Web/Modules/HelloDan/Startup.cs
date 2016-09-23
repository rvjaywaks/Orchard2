using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Orchard;

namespace HelloDan
{
    public class Startup : StartupBase
    {
        public override void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {
            routes.MapAreaRoute
            (
                name: "Dan",
                area: "HelloDan",
                template: "dan",
                controller: "Home",
                action: "Index"
            );
        }

        public override void ConfigureServices(IServiceCollection serviceCollection)
        {

        }
    }
}
