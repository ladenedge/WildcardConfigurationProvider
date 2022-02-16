using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace WildcardConfigurationProvider
{
    /// <summary>
    /// A configuration provider that acts as an adapter for underlying <see cref="FileConfigurationProvider"/>
    /// providers that represent individual files matching the wildcard.
    /// </summary>
    public class WildcardConfigurationProvider : ConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance with the specified source.
        /// </summary>
        /// <param name="source">The source settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="source"/> was null.</exception>
        public WildcardConfigurationProvider(WildcardConfigurationSource source)
        {
            Source = source ?? throw new ArgumentNullException(nameof(source));
            if (Source.ProviderFactory == null)
                throw new InvalidOperationException("Wildcard source must have a provider factory");

            Load();
        }

        /// <summary>
        /// The source settings for this provider.
        /// </summary>
        public WildcardConfigurationSource Source { get; }
        
        /// <summary>
        /// The underlying, individual file providers backing this provider.
        /// </summary>
        IEnumerable<FileConfigurationProvider> Providers { get; set; } = new List<FileConfigurationProvider>();

        /// <summary>
        /// Loads individual providers for each file that matches the wildcard specified in <see cref="Source"/>.
        /// </summary>
        /// <exception cref="InvalidOperationException">The <see cref="IFileProvider"/> in <see cref="Source"/> did not represent a physical file provider.</exception>
        public override void Load()
        {
            if (!(Source.FileProvider is PhysicalFileProvider directory))
                throw new InvalidOperationException($"FileProvider was unexpected type '{Source.FileProvider.GetType().Name}'");

            var providers = new List<FileConfigurationProvider>();

            foreach (var file in Directory.GetFiles(directory.Root, Source.Path))
            {
                var filename = Path.GetFileName(file);
                var provider = Source.ProviderFactory(Source, filename);
                provider.Load();
                providers.Add(provider);
            }

            Providers = providers;
        }

        /// <summary>
        /// Returns the list of keys in all underlying file providers.
        /// </summary>
        /// <param name="earlierKeys">The earlier keys that other providers contain.</param>
        /// <param name="parentPath">The path for the parent IConfiguration.</param>
        /// <returns>The list of keys for this provider.</returns>
        public override IEnumerable<string> GetChildKeys(IEnumerable<string> earlierKeys, string parentPath)
        {
            return Providers.SelectMany(p => p.GetChildKeys(earlierKeys, parentPath))
                            .Concat(base.GetChildKeys(earlierKeys, parentPath));
        }

        /// <summary>
        /// Attempts to find a value with the specified key.
        /// </summary>
        /// <param name="key">The key to lookup.</param>
        /// <param name="value">When this method returns, contains the value found at <paramref name="key"/>, if one is found.</param>
        /// <returns><b>true</b> if key has a value, <b>false</b> otherwise.</returns>
        public override bool TryGet(string key, out string value)
        {
            foreach (var provider in Providers)
                if (provider.TryGet(key, out value))
                    return true;
            
            return base.TryGet(key, out value);
        }

        /// <summary>
        /// Generates a string representing this provider name and relevant details.
        /// </summary>
        /// <returns>The configuration name and source path.</returns>
        public override string ToString()
        {
            return $"{GetType().Name} for '{Source.Path}'";
        }
    }
}
