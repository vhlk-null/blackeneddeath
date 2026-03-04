using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.Pagination;
using FluentAssertions;
using Library.API.Endpoints.Albums;
using Library.Application.Albums.Commands.CreateAlbum;
using Library.Application.Albums.Commands.DeleteAlbums;
using Library.Application.Albums.Queries.GetAlbums;
using Library.Application.Dtos;
using Moq;
using Xunit;

namespace Library.APITests.Endpoints;

public class AlbumEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateAlbum_ValidRequest_Returns201WithId()
    {
        var albumId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateAlbumCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateAlbumResult>(new CreateAlbumResult(albumId)));

        var request = new
        {
            Album = new
            {
                Id = Guid.NewGuid(),
                Title = "Symbolic",
                ReleaseDate = 1995,
                CoverUrl = (string?)null,
                Type = 0,
                Format = 0,
                Label = "Roadrunner Records",
                Bands = Array.Empty<object>(),
                Countries = Array.Empty<object>(),
                StreamingLinks = Array.Empty<object>(),
                Tracks = Array.Empty<object>(),
                Genres = Array.Empty<object>()
            }
        };

        var response = await _client.PostAsJsonAsync("/albums", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateAlbumResponse>();
        body!.Id.Should().Be(albumId);
    }

    [Fact]
    public async Task DeleteAlbum_ValidId_Returns200WithSuccess()
    {
        var albumId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteAlbumCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteAlbumResult>(new DeleteAlbumResult(true)));

        var response = await _client.DeleteAsync($"/albums/{albumId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeleteAlbumResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetAlbums_Returns200WithPaginatedResult()
    {
        var appResult = new Library.Application.Albums.Queries.GetAlbums.GetAlbumsResult(
            new PaginatedResult<AlbumDto>(0, 10, 0, []));
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetAlbumsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Library.Application.Albums.Queries.GetAlbums.GetAlbumsResult>(appResult));

        var response = await _client.GetAsync("/albums?pageIndex=0&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
