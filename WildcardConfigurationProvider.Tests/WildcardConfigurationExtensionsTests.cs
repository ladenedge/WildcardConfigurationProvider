using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Moq;
using NUnit.Framework;

namespace WildcardConfigurationProvider.Tests;

public class WildcardConfigurationExtensionsTests
{
    [AutoMoqData]
    public void AddJsonWildcard_ThrowsOnNullBuilder(string path)
    {
        IConfigurationBuilder builder = null;
        Assert.That(() => builder.AddJsonWildcard(path), Throws.ArgumentNullException);
    }

    [Test]
    public void AddJsonWildcard_ThrowsOnNullPath([Values(null, "")] string path)
    {
        var builder = Mock.Of<IConfigurationBuilder>();
        Assert.That(() => builder.AddJsonWildcard(path), Throws.ArgumentNullException);
    }

    [AutoMoqData]
    public void AddJsonWildcard_ReloadOnChangeDefaultsToFalse(IConfigurationBuilder builder, string path)
    {
        builder.AddJsonWildcard(path);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.ReloadOnChange == false)));
    }

    [AutoMoqData]
    public void AddJsonWildcard_FileProviderResolvesWhenNull(IConfigurationBuilder builder)
    {
        // Path must actually exist for the provider to resolve.
        var path = TestSupport.LocateTestFile("appsettings.a.json");
        builder.AddJsonWildcard(path);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.FileProvider != null)));

    }

    [AutoMoqData]
    public void AddJsonWildcard_DelegatesPath(IConfigurationBuilder builder, string path)
    {
        builder.AddJsonWildcard(path);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.Path == path)));
    }

    [AutoMoqData]
    public void AddJsonWildcard_DelegatesReloadOnChange(IConfigurationBuilder builder, string path, bool reload)
    {
        builder.AddJsonWildcard(path, reload);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.ReloadOnChange == reload)));
    }

    [AutoMoqData]
    public void AddJsonWildcard_DelegatesFileProvider(IConfigurationBuilder builder, IFileProvider provider, string path, bool reload)
    {
        builder.AddJsonWildcard(provider, path, reload);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.FileProvider == provider)));
    }

    [AutoMoqData]
    public void AddJsonWildcard_UnderlyingSourceIsOptional(IConfigurationBuilder builder, string path)
    {
        builder.AddJsonWildcard(path);
        Mock.Get(builder).Verify(b => b.Add(It.Is<WildcardConfigurationSource>(s => s.Optional == true)));
    }

    [AutoMoqData]
    public void AddJsonWildcard_ReturnsBuilder(IConfigurationBuilder builder, string path)
    {
        Mock.Get(builder).Setup(b => b.Add(It.IsAny<IConfigurationSource>())).Returns(builder);
        var returnedBuilder = builder.AddJsonWildcard(path);
        Assert.That(returnedBuilder, Is.SameAs(builder));
    }
}