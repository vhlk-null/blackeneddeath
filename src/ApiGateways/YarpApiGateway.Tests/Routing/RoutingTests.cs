using System.Net;
using FluentAssertions;
using Xunit;

namespace YarpApiGateway.Tests.Routing;

public class RoutingTests(YarpGatewayWebAppFactory factory) : IClassFixture<YarpGatewayWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("/library/albums")]
    [InlineData("/library/bands")]
    [InlineData("/library/genres")]
    public async Task LibraryRoutes_MatchAndAttemptProxy_DoNotReturn404(string path)
    {
        HttpResponseMessage response = await _client.GetAsync(path, TestContext.Current.CancellationToken);

        // Route matched — YARP tried to forward (upstream unreachable = 502/503, not 404)
        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("/usercontent/users")]
    [InlineData("/usercontent/favorites")]
    public async Task UserContentRoutes_MatchAndAttemptProxy_DoNotReturn404(string path)
    {
        HttpResponseMessage response = await _client.GetAsync(path, TestContext.Current.CancellationToken);

        response.StatusCode.Should().NotBe(HttpStatusCode.NotFound);
    }

    [Theory]
    [InlineData("/unknown")]
    [InlineData("/api/something")]
    [InlineData("/v1/albums")]
    public async Task UnknownRoutes_NoMatch_Return404(string path)
    {
        HttpResponseMessage response = await _client.GetAsync(path, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
