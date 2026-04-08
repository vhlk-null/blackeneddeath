using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Library.API.Endpoints.Countries;
using Library.Application.Dtos;
using Library.Application.Services.Countries.Commands.CreateCountry;
using Library.Application.Services.Countries.Commands.DeleteCountry;
using Library.Application.Services.Countries.Queries.GetCountries;
using Moq;
using Xunit;
using GetCountriesResult = Library.Application.Services.Countries.Queries.GetCountries.GetCountriesResult;

namespace Library.APITests.Endpoints;

public class CountryEndpointsTests(LibraryWebAppFactory factory) : IClassFixture<LibraryWebAppFactory>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Fact]
    public async Task CreateCountry_ValidRequest_Returns201WithId()
    {
        Guid countryId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<CreateCountryCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<CreateCountryResult>(new CreateCountryResult(countryId)));

        HttpResponseMessage response = await _client.PostAsJsonAsync("/countries", new { Name = "Norway", Code = "NO" });

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        CreateCountryResponse? body = await response.Content.ReadFromJsonAsync<CreateCountryResponse>();
        body!.Id.Should().Be(countryId);
    }

    [Fact]
    public async Task DeleteCountry_ValidId_Returns200WithSuccess()
    {
        Guid countryId = Guid.NewGuid();
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<DeleteCountryCommand>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<DeleteCountryResult>(new DeleteCountryResult(true)));

        HttpResponseMessage response = await _client.DeleteAsync($"/countries/{countryId}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        DeleteCountryResponse? body = await response.Content.ReadFromJsonAsync<DeleteCountryResponse>();
        body!.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetCountries_Returns200WithList()
    {
        GetCountriesResult appResult = new GetCountriesResult([]);
        factory.SenderMock
            .Setup(s => s.Send(It.IsAny<GetCountriesQuery>(), It.IsAny<CancellationToken>()))
            .Returns(new ValueTask<GetCountriesResult>(appResult));

        HttpResponseMessage response = await _client.GetAsync("/countries");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
