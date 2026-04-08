using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using BuildingBlocks.Pagination;
using FluentAssertions;
using Library.API.Endpoints.Albums;
using Library.Application.Dtos;
using Library.Application.Services.Albums.Commands.CreateAlbum;
using Library.Application.Services.Albums.Commands.DeleteAlbums;
using Library.Application.Services.Albums.Queries.GetAlbums;
using Moq;
using Xunit;
using GetAlbumsResult = Library.Application.Services.Albums.Queries.GetAlbums.GetAlbumsResult;

namespace Library.APITests.Endpoints;

public class AlbumEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateAlbum_ValidRequest_Returns201WithId()
    {
        Guid albumId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateAlbumCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateAlbumResult>(new CreateAlbumResult(albumId)));

        string albumJson = JsonSerializer.Serialize(new
        {
            Id = Guid.NewGuid(),
            Title = "Symbolic",
            ReleaseDate = 1995,
            CoverUrl = (string?)null,
            Type = 0,
            Format = 0,
            Label = (object?)null,
            Bands = Array.Empty<object>(),
            Countries = Array.Empty<object>(),
            StreamingLinks = Array.Empty<object>(),
            Tracks = Array.Empty<object>(),
            Genres = Array.Empty<object>()
        });

        MultipartFormDataContent form = new MultipartFormDataContent();
        form.Add(new StringContent(albumJson, Encoding.UTF8, "application/json"), "album");

        HttpResponseMessage response = await _client.PostAsync("/albums", form);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        CreateAlbumResponse? body = await response.Content.ReadFromJsonAsync<CreateAlbumResponse>();
        body!.Id.Should().Be(albumId);
    }

    [Fact]
    public async Task DeleteAlbum_ValidId_Returns200WithSuccess()
    {
        Guid albumId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteAlbumCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteAlbumResult>(new DeleteAlbumResult(true)));

        HttpResponseMessage response = await _client.DeleteAsync($"/albums/{albumId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        DeleteAlbumResponse? body = await response.Content.ReadFromJsonAsync<DeleteAlbumResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetAlbums_Returns200WithPaginatedResult()
    {
        GetAlbumsResult appResult = new GetAlbumsResult(
            new PaginatedResult<AlbumDto>(0, 10, 0, []));
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetAlbumsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetAlbumsResult>(appResult));

        HttpResponseMessage response = await _client.GetAsync("/albums?pageIndex=0&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
