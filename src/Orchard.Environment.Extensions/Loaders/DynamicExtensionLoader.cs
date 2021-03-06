﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Orchard.Environment.Extensions.Folders;
using Orchard.Environment.Extensions.Models;
using Orchard.FileSystem;

namespace Orchard.Environment.Extensions.Loaders
{
    public class DynamicExtensionLoader : IExtensionLoader
    {
        private readonly string[] ExtensionsSearchPaths;

        private readonly IOrchardFileSystem _fileSystem;
        private readonly IExtensionLibraryService _extensionLibraryService;
        private readonly ILogger _logger;

        public DynamicExtensionLoader(
            IOptions<ExtensionHarvestingOptions> optionsAccessor,
            IOrchardFileSystem fileSystem,
            IExtensionLibraryService extensionLibraryService,
            ILogger<DynamicExtensionLoader> logger)
        {
            ExtensionsSearchPaths = optionsAccessor.Value.ExtensionLocationExpanders.SelectMany(x => x.SearchPaths).ToArray();
            _fileSystem = fileSystem;
            _extensionLibraryService = extensionLibraryService;
            _logger = logger;
        }

        public string Name => GetType().Name;

        public int Order => 50;

        public void ExtensionActivated(ExtensionLoadingContext ctx, ExtensionDescriptor extension)
        {
        }

        public void ExtensionDeactivated(ExtensionLoadingContext ctx, ExtensionDescriptor extension)
        {
        }

        public bool IsCompatibleWithModuleReferences(ExtensionDescriptor extension, IEnumerable<ExtensionProbeEntry> references)
        {
            return true;
        }

        public ExtensionEntry Load(ExtensionDescriptor descriptor)
        {
            if (!ExtensionsSearchPaths.Contains(descriptor.Location))
            {
                return null;
            }

            try
            {
                var assembly = _extensionLibraryService.LoadDynamicExtension(descriptor);
            
                if (assembly == null)
                {
                    return null;
                }

                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Loaded referenced dynamic extension \"{0}\": assembly name=\"{1}\"", descriptor.Name, assembly.FullName);
                }

                return new ExtensionEntry
                {
                    Descriptor = descriptor,
                    Assembly = assembly,
                    ExportedTypes = assembly.ExportedTypes
                };
            }
            catch
            {
                return null;
            }
       }

        public ExtensionProbeEntry Probe(ExtensionDescriptor descriptor)
        {
            return null;
        }

        public void ReferenceActivated(ExtensionLoadingContext context, ExtensionReferenceProbeEntry referenceEntry)
        {
        }

        public void ReferenceDeactivated(ExtensionLoadingContext context, ExtensionReferenceProbeEntry referenceEntry)
        {
        }
    }
}