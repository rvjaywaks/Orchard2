using Microsoft.AspNetCore.Mvc.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Orchard.Hosting
{
    /// <summary>
    /// These services are registered on the tenant service collection
    /// </summary>
    public class ShellModule : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
        }
    }
}