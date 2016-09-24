﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Orchard.Environment.Shell;
using Orchard.Roles.Services;
using Orchard.Security;
using Orchard.Security.Services;

namespace Orchard.Roles
{
    public class Startup : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.TryAddScoped<RoleManager<Role>>();
            services.TryAddScoped<IRoleStore<Role>, RoleStore>();
            services.TryAddScoped<IRoleProvider, RoleStore>();

            services.AddScoped<RoleUpdater>();
            services.AddScoped<IFeatureEventHandler>(sp => sp.GetRequiredService<RoleUpdater>());
            services.AddScoped<IAuthorizationHandler, RolesPermissionsHandler>();
        }
    }
}
