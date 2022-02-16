using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace WildcardConfigurationProvider.Tests;

public class WildcardConfigurationProviderTests
{
    [Test]
    public void AddJsonWildcard_ReadsMultipleFiles()
    {
        var builder = new ConfigurationBuilder();
        var path = TestSupport.LocateTestFile("appsettings.*.json");
        
        var config = builder.AddJsonWildcard(path).Build();

        Assert.That(config["key1"], Is.EqualTo("value1"));
        Assert.That(config["key2"], Is.EqualTo("value2"));
    }
}