using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.Exceptions;
using FluentAssertions;
using Moq;
using UserContent.API.Endpoints.FavoriteAlbums;
using Xunit;

namespace UserContent.APITests.Endpoints;

public class FavoriteAlbumsControllerTests(UserContentWebAppFactory factory) : IClassFixture<UserContentWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AddAlbumToFavorite_ValidRequest_Returns201WithUserId()
    {
        var userId = Guid.NewGuid();
        var request = new AddAlbumToFavoriteRequest(Guid.NewGuid(), userId);
        factory.ServiceMock
            .Setup(s => s.AddFavoriteAlbumAsync(userId, request.AlbumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        var response = await _client.PostAsJsonAsync("/favoriteAlbums", request, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<AddAlbumToFavoriteResponse>(TestContext.Current.CancellationToken);
        body!.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task DeleteFavoriteAlbum_ValidRequest_Returns200WithSuccess()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        factory.ServiceMock
            .Setup(s => s.DeleteFavoriteAlbumAsync(userId, albumId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var response = await _client.DeleteAsync($"/favoriteAlbums?userId={userId}&albumId={albumId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeleteFavoriteAlbumResponse>(TestContext.Current.CancellationToken);
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteFavoriteAlbum_ServiceThrowsNotFound_Returns404()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        factory.ServiceMock
            .Setup(s => s.DeleteFavoriteAlbumAsync(userId, albumId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("FavoriteAlbum", albumId));

        var response = await _client.DeleteAsync($"/favoriteAlbums?userId={userId}&albumId={albumId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
