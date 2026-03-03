using System.Net;
using System.Net.Http.Json;
using BuildingBlocks.Pagination;
using FluentAssertions;
using Library.API.Endpoints.Genres;
using Library.Application.Dtos;
using Library.Application.Genres.Commands.CreateGenre;
using Library.Application.Genres.Commands.DeleteGenre;
using Library.Application.Genres.Queries.GetGenres;
using Moq;
using Xunit;

namespace Library.APITests.Endpoints;

public class GenreEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateGenre_ValidRequest_Returns201WithId()
    {
        var genreId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateGenreCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateGenreResult>(new CreateGenreResult(genreId)));

        var response = await _client.PostAsJsonAsync("/genres", new { Name = "Death Metal", ParentGenreId = (Guid?)null });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateGenreResponse>();
        body!.Id.Should().Be(genreId);
    }

    [Fact]
    public async Task DeleteGenre_ValidId_Returns200WithSuccess()
    {
        var genreId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteGenreCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteGenreResult>(new DeleteGenreResult(true)));

        var response = await _client.DeleteAsync($"/genres/{genreId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeleteGenreResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetGenres_Returns200WithPaginatedResult()
    {
        var appResult = new Library.Application.Genres.Queries.GetGenres.GetGenresResult(
            new PaginatedResult<GenreDetailDto>(0, 10, 0, []));
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetGenresQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<Library.Application.Genres.Queries.GetGenres.GetGenresResult>(appResult));

        var response = await _client.GetAsync("/genres?pageIndex=0&pageSize=10");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
