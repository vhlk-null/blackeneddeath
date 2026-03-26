using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Library.API.Endpoints.Tags;
using Library.Application.Dtos;
using Library.Application.Services.Tags.Commands.CreateTag;
using Library.Application.Services.Tags.Commands.DeleteTag;
using Library.Application.Services.Tags.Queries.GetTags;
using Moq;
using Xunit;
using GetTagsResult = Library.Application.Services.Tags.Queries.GetTags.GetTagsResult;

namespace Library.APITests.Endpoints;

public class TagEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateTag_ValidRequest_Returns201WithId()
    {
        var tagId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateTagCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateTagResult>(new CreateTagResult(tagId)));

        var response = await _client.PostAsJsonAsync("/tags", new { Name = "Atmospheric" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await response.Content.ReadFromJsonAsync<CreateTagResponse>();
        body!.Id.Should().Be(tagId);
    }

    [Fact]
    public async Task DeleteTag_ValidId_Returns200WithSuccess()
    {
        var tagId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteTagCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteTagResult>(new DeleteTagResult(true)));

        var response = await _client.DeleteAsync($"/tags/{tagId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var body = await response.Content.ReadFromJsonAsync<DeleteTagResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetTags_Returns200WithList()
    {
        var appResult = new GetTagsResult([]);
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetTagsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetTagsResult>(appResult));

        var response = await _client.GetAsync("/tags");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
