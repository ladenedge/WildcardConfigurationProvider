# WildcardConfigurationProvider

A file-based .NET configuration provider that supports wildcard notation.
Technically, this provider acts as an adapter for a set of [FileConfigurationProviders](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration-providers#file-configuration-provider).

ℹ️ At the moment, only JSON is supported for the underlying file type.  XML and INI are forthcoming.

## Installation

```
Install-Package WildcardConfigurationProvider
```

## Usage

Use the wildcard provider just like any other `FileConfigurationProvider`, except that it supports wildcard notation.

```cs
var builder = new ConfigurationBuilder();
var config = builder.AddJsonWildcard("appsettings.*.json").Build();
```

⚠️ While this configuration provider supports `reloadOnChange`, it only watches those config files that are present at
configuration build time.  In other words, it doesn't watch the directory for new, additional matches to the wildcard.
To include newly-created files in your configuration, a `config.Reload()` is required.
