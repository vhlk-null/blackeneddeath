using System.Net;
using FluentAssertions;
using Xunit;

namespace YarpApiGateway.Tests.RateLimiting;

public class RateLimiterTests
{
    [Fact]
    public async Task RateLimiter_ExceedsLimit_Returns429()
    {
        // Each test gets its own factory so the rate limit window resets
        await using var factory = new YarpGatewayWebAppFactory();
        using var client = factory.CreateClient();

        const int permitLimit = 5;
        var responses = new List<HttpResponseMessage>();

        for (int i = 0; i <= permitLimit; i++)
            responses.Add(await client.GetAsync("/library/albums"));

        // First 5 requests: route matched (not 429)
        responses.Take(permitLimit).Should().NotContain(r => r.StatusCode == HttpStatusCode.TooManyRequests);

        // 6th request: rate limit exceeded
        responses.Last().StatusCode.Should().Be(HttpStatusCode.TooManyRequests);
    }

    [Fact]
    public async Task RateLimiter_WithinLimit_AllRequestsPass()
    {
        await using var factory = new YarpGatewayWebAppFactory();
        using var client = factory.CreateClient();

        const int permitLimit = 5;
        var responses = new List<HttpResponseMessage>();

        for (int i = 0; i < permitLimit; i++)
            responses.Add(await client.GetAsync("/library/albums"));

        responses.Should().NotContain(r => r.StatusCode == HttpStatusCode.TooManyRequests);
    }
}
