using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Configuration;
using Moq;
using System;

namespace WildcardConfigurationProvider.Tests;

public class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute() : base(CustomizeFixture) { }

    public static IFixture CustomizeFixture()
    {
        var fixture = new Fixture().Customize(new AutoMoqCustomization());

        var builderMock = fixture.Freeze<Mock<IConfigurationBuilder>>();
        fixture.Inject(builderMock.Object);

        return fixture;
    }
}
