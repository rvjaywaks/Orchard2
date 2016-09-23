using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Orchard.Environment.Extensions;
using Orchard.Environment.Shell.Descriptor.Models;

namespace Orchard.Environment.Shell.Descriptor.Settings
{
    public class SingleShellDescriptorManager : IShellDescriptorManager
    {
        private readonly IExtensionManager _extensionManager;
        private ShellDescriptor _shellDescriptor;

        public SingleShellDescriptorManager(IExtensionManager extensionManager)
        {
            _extensionManager = extensionManager;
        }

        public Task<ShellDescriptor> GetShellDescriptorAsync()
        {
            if (_shellDescriptor == null)
            {
                _shellDescriptor = new ShellDescriptor
                {
                    Features = _extensionManager.AvailableFeatures().Select(x => new ShellFeature { Name = x.Id }).ToList()
                };
            }

            return Task.FromResult(_shellDescriptor);
        }

        public Task UpdateShellDescriptorAsync(int priorSerialNumber, IEnumerable<ShellFeature> enabledFeatures, IEnumerable<ShellParameter> parameters)
        {
            return Task.CompletedTask;
        }
    }
}