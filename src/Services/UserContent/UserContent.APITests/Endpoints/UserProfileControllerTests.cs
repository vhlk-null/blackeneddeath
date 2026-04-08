using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Moq;
using UserContent.Application.Dtos;
using UserContent.Application.Exceptions;
using Xunit;

namespace UserContent.APITests.Endpoints;

public class UserProfileControllerTests : IClassFixture<UserContentWebAppFactory>
{
    private readonly HttpClient _client;
    private readonly UserContentWebAppFactory _factory;

    public UserProfileControllerTests(UserContentWebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetUserProfile_ExistingUser_Returns200WithDto()
    {
        Guid userId = Guid.NewGuid();
        UserProfileDto dto = new UserProfileDto(userId, "metal_head", "user@example.com",
            null, DateTime.UtcNow, null, null, 0, 0, 0, [], []);
        _factory.ServiceMock
            .Setup(s => s.GetUserProfileAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dto);

        HttpResponseMessage response = await _client.GetAsync($"/profile/{userId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        UserProfileDto? body = await response.Content.ReadFromJsonAsync<UserProfileDto>(TestContext.Current.CancellationToken);
        body!.UserId.Should().Be(userId);
        body.Username.Should().Be("metal_head");
    }

    [Fact]
    public async Task GetUserProfile_UserNotFound_Returns404()
    {
        Guid userId = Guid.NewGuid();
        _factory.ServiceMock
            .Setup(s => s.GetUserProfileAsync(userId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new UserProfileNotFoundException(userId));

        HttpResponseMessage response = await _client.GetAsync($"/profile/{userId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
