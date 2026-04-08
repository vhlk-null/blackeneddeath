using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Library.API.Endpoints.Genres;
using Library.Application.Dtos;
using Library.Application.Services.Genres.Commands.CreateGenre;
using Library.Application.Services.Genres.Commands.DeleteGenre;
using Library.Application.Services.Genres.Queries.GetGenres;
using Moq;
using Xunit;
using GetGenresResult = Library.Application.Services.Genres.Queries.GetGenres.GetGenresResult;

namespace Library.APITests.Endpoints;

public class GenreEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateGenre_ValidRequest_Returns201WithId()
    {
        Guid genreId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateGenreCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateGenreResult>(new CreateGenreResult(genreId)));

        HttpResponseMessage response = await _client.PostAsJsonAsync("/genres", new { Name = "Death Metal", ParentGenreId = (Guid?)null });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        CreateGenreResponse? body = await response.Content.ReadFromJsonAsync<CreateGenreResponse>();
        body!.Id.Should().Be(genreId);
    }

    [Fact]
    public async Task DeleteGenre_ValidId_Returns200WithSuccess()
    {
        Guid genreId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteGenreCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteGenreResult>(new DeleteGenreResult(true)));

        HttpResponseMessage response = await _client.DeleteAsync($"/genres/{genreId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        DeleteGenreResponse? body = await response.Content.ReadFromJsonAsync<DeleteGenreResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetGenres_Returns200WithList()
    {
        GetGenresResult appResult = new GetGenresResult([]);
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetGenresQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetGenresResult>(appResult));

        HttpResponseMessage response = await _client.GetAsync("/genres");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
