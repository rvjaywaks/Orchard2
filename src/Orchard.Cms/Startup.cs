using Microsoft.AspNetCore.Mvc.Modules;
using Microsoft.Extensions.DependencyInjection;
using Orchard.BackgroundTasks;
using Orchard.Data;
using Orchard.DeferredTasks;
using Orchard.ResourceManagement;

namespace Orchard.Cms
{
    /// <summary>
    /// These services are registered on the tenant service collection
    /// </summary>
    public class ShellModule : StartupBase
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            services.AddDeferredTasks();
            services.AddDataAccess();
            services.AddBackgroundTasks();
            services.AddResourceManagement();
        }
    }
}
