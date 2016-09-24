﻿using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc.Modules
{
    public abstract class StartupBase : IStartup
    {
        /// <inheritdoc />
        public virtual void ConfigureServices(IServiceCollection services)
        {

        }

        /// <inheritdoc />
        public virtual void Configure(IApplicationBuilder builder, IRouteBuilder routes, IServiceProvider serviceProvider)
        {

        }
    }
}
