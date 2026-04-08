using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Library.API.Endpoints.Labels;
using Library.Application.Dtos;
using Library.Application.Services.Labels.Commands.CreateLabel;
using Library.Application.Services.Labels.Commands.DeleteLabel;
using Library.Application.Services.Labels.Queries.GetLabels;
using Moq;
using Xunit;
using GetLabelsResult = Library.Application.Services.Labels.Queries.GetLabels.GetLabelsResult;

namespace Library.APITests.Endpoints;

public class LabelEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateLabel_ValidRequest_Returns201WithId()
    {
        Guid labelId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateLabelCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateLabelResult>(new CreateLabelResult(labelId)));

        HttpResponseMessage response = await _client.PostAsJsonAsync("/labels", new { Name = "Nuclear Blast" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        CreateLabelResponse? body = await response.Content.ReadFromJsonAsync<CreateLabelResponse>();
        body!.Id.Should().Be(labelId);
    }

    [Fact]
    public async Task DeleteLabel_ValidId_Returns200WithSuccess()
    {
        Guid labelId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteLabelCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteLabelResult>(new DeleteLabelResult(true)));

        HttpResponseMessage response = await _client.DeleteAsync($"/labels/{labelId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        DeleteLabelResponse? body = await response.Content.ReadFromJsonAsync<DeleteLabelResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetLabels_Returns200WithList()
    {
        GetLabelsResult appResult = new GetLabelsResult([]);
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetLabelsQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetLabelsResult>(appResult));

        HttpResponseMessage response = await _client.GetAsync("/labels");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
