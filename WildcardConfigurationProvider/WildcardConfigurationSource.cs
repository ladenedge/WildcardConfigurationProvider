using Microsoft.Extensions.Configuration;
using System;

namespace WildcardConfigurationProvider
{
    /// <summary>
    /// Represents a wildcard spec as an <see cref="IConfigurationSource"/>.
    /// </summary>
    public class WildcardConfigurationSource : FileConfigurationSource
    {
        /// <summary>
        /// Factory to create the underlying file providers.
        /// </summary>
        public Func<FileConfigurationSource, string, FileConfigurationProvider> ProviderFactory { get; set; }

        /// <summary>
        /// Builds the <see cref="WildcardConfigurationProvider"/> for this source.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/>.</param>
        /// <returns>A <see cref="WildcardConfigurationProvider"/></returns>
        public override IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            EnsureDefaults(builder);
            return new WildcardConfigurationProvider(this);
        }
    }
}
