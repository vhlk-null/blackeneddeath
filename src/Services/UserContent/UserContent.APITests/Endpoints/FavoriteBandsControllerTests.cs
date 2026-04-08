using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.Exceptions;
using FluentAssertions;
using Moq;
using UserContent.API.Endpoints.FavoriteBands;
using Xunit;

namespace UserContent.APITests.Endpoints;

public class FavoriteBandsControllerTests(UserContentWebAppFactory factory) : IClassFixture<UserContentWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task AddBandToFavorite_ValidRequest_Returns201WithUserId()
    {
        Guid userId = Guid.NewGuid();
        AddBandToFavoriteRequest request = new AddBandToFavoriteRequest(Guid.NewGuid(), userId);
        factory.ServiceMock
            .Setup(s => s.AddFavoriteBandAsync(userId, request.BandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userId);

        HttpResponseMessage response = await _client.PostAsJsonAsync("/favoriteBands", request, TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        AddBandToFavoriteResponse? body = await response.Content.ReadFromJsonAsync<AddBandToFavoriteResponse>(TestContext.Current.CancellationToken);
        body!.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task DeleteFavoriteBand_ValidRequest_Returns200WithSuccess()
    {
        Guid userId = Guid.NewGuid();
        Guid bandId = Guid.NewGuid();
        factory.ServiceMock
            .Setup(s => s.DeleteFavoriteBandAsync(userId, bandId, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        HttpResponseMessage response = await _client.DeleteAsync($"/favoriteBands?userId={userId}&bandId={bandId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        DeleteFavoriteBandResponse? body = await response.Content.ReadFromJsonAsync<DeleteFavoriteBandResponse>(TestContext.Current.CancellationToken);
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteFavoriteBand_ServiceThrowsNotFound_Returns404()
    {
        Guid userId = Guid.NewGuid();
        Guid bandId = Guid.NewGuid();
        factory.ServiceMock
            .Setup(s => s.DeleteFavoriteBandAsync(userId, bandId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException("FavoriteBand", bandId));

        HttpResponseMessage response = await _client.DeleteAsync($"/favoriteBands?userId={userId}&bandId={bandId}", TestContext.Current.CancellationToken);

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
