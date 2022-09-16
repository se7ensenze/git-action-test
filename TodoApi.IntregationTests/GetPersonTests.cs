using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json.Linq;

namespace TodoApi.IntegrationTests;

public class GetPersonTests
    :IClassFixture<GetPersonTestsFixture>
{
    private readonly GetPersonTestsFixture _fixture;

    public GetPersonTests(GetPersonTestsFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task Test1()
    {
        //Arrange
        var client = _fixture.CreateClient();
        
        //Action
        var result = await client.GetAsync("/person");
        
        //Assert
        result.IsSuccessStatusCode.Should().BeTrue();
        result.Content.Should().NotBeNull();

        var json = await result.Content.ReadAsStringAsync();
        var root = JObject.Parse(json);
        root.Properties().Should().Contain(jp => jp.Name == "persons");
    }
}

public class GetPersonTestsFixture
{
    private readonly WebApplicationFactory<Program> _factory;
    
    public GetPersonTestsFixture()
    {
        _factory = new WebApplicationFactory<Program>();
        _factory.ClientOptions.AllowAutoRedirect = false;
    }

    public HttpClient CreateClient() => _factory.CreateClient();
}