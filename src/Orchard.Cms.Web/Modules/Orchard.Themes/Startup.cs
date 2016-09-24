﻿using Microsoft.AspNetCore.Mvc.Modules;
using Microsoft.Extensions.DependencyInjection;
using Orchard.Recipes;
using Orchard.Security.Permissions;
using Orchard.Themes.Recipes;

namespace Orchard.Themes
{
    /// <summary>
    /// These services are registered on the tenant service collection
    /// </summary>
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddRecipeExecutionStep<ThemesStep>();
            services.AddScoped<IPermissionProvider, Permissions>();
        }
    }
}
