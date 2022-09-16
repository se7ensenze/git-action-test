using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace EchoApi.IntegrationTests;

public class EchoTests : IClassFixture<EchoTestsFixture>
{
    private readonly EchoTestsFixture _fixture;

    public EchoTests(EchoTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task GetEchoShouldSucceed()
    {
        var httpClient = _fixture.CreateClient();

        var result = await httpClient.GetAsync("/echo?personName=Test");

        Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
}

public class EchoTestsFixture
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public EchoTestsFixture()
    {
        _factory = new WebApplicationFactory<Program>();
        _factory.ClientOptions.AllowAutoRedirect = false;
    }

    public HttpClient CreateClient() => _factory.CreateClient();
}