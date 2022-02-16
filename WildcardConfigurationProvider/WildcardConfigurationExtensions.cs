using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;

namespace WildcardConfigurationProvider
{
    /// <summary>
    /// Extension methods for adding <see cref="WildcardConfigurationProvider"/>.
    /// </summary>
    public static class WildcardConfigurationExtensions
    {
        /// <summary>
        /// Adds the wildcard configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path with optional wildcards relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonWildcard(this IConfigurationBuilder builder, string path)
        {
            return AddJsonWildcard(builder, provider: null, path: path, reloadOnChange: false);
        }

        /// <summary>
        /// Adds the wildcard configuration provider at <paramref name="path"/> to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="path">Path with optional wildcards relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonWildcard(this IConfigurationBuilder builder, string path, bool reloadOnChange)
        {
            return AddJsonWildcard(builder, provider: null, path: path, reloadOnChange: reloadOnChange);
        }

        /// <summary>
        /// Adds a wildcard configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="provider">The <see cref="IFileProvider"/> to use to access the file.</param>
        /// <param name="path">Path with optional wildcards relative to the base path stored in 
        /// <see cref="IConfigurationBuilder.Properties"/> of <paramref name="builder"/>.</param>
        /// <param name="reloadOnChange">Whether the configuration should be reloaded if the file changes.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonWildcard(this IConfigurationBuilder builder, IFileProvider provider, string path, bool reloadOnChange)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            return builder.AddJsonWildcard(s =>
            {
                s.ProviderFactory = (src, file) =>
                {
                    var jsonSource = new JsonConfigurationSource
                    {
                        FileProvider = src.FileProvider,
                        OnLoadException = src.OnLoadException,
                        Optional = false,
                        Path = file,
                        ReloadDelay = src.ReloadDelay,
                        ReloadOnChange = src.ReloadOnChange,
                    };
                    return new JsonConfigurationProvider(jsonSource);
                };
                s.FileProvider = provider;
                s.Optional = true;
                s.Path = path;
                s.ReloadOnChange = reloadOnChange;
                s.ResolveFileProvider();
            });
        }

        /// <summary>
        /// Adds a wildcard configuration source to <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="IConfigurationBuilder"/> to add to.</param>
        /// <param name="configureSource">Configures the source.</param>
        /// <returns>The <see cref="IConfigurationBuilder"/>.</returns>
        public static IConfigurationBuilder AddJsonWildcard(this IConfigurationBuilder builder, Action<WildcardConfigurationSource> configureSource)
            => builder.Add(configureSource);
    }
}
