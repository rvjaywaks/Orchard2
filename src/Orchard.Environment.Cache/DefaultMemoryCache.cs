﻿using Microsoft.AspNetCore.Mvc.Modules;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;

namespace Orchard.Environment.Cache
{
    public class DefaultMemoryCache : StartupBase
    {
        public override void ConfigureServices(IServiceCollection serviceCollection)
        {
            // MVC is already registering IMemoryCache as host singleton. We are registering it again
            // in this module so that there is one instance for each tenant.
            serviceCollection.Add(ServiceDescriptor.Singleton<IMemoryCache, MemoryCache>());


            // LocalCache is registered as transient as its implementation resolves IMemoryCache, thus
            // there is no state to keep in its instance.
            serviceCollection.Add(ServiceDescriptor.Transient<IDistributedCache, MemoryDistributedCache>());
        }
    }
}
