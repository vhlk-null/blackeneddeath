using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Countries.Commands.UpdateCountry;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Countries;

public class UpdateCountryHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Country>> _countriesDbSetMock;
    private readonly UpdateCountryCommandHandler _handler;

    public UpdateCountryHandlerTests()
    {
        _countriesDbSetMock = MockDbSetFactory.Create<Country>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Countries).Returns(_countriesDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateCountryCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCountry_UpdatesAndReturnsSuccess()
    {
        Guid countryId = Guid.NewGuid();
        Country country = Country.Create(CountryId.Of(countryId), "Norway", "NO");
        _countriesDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Country?>(country));

        UpdateCountryResult result = await _handler.Handle(
            new UpdateCountryCommand(countryId, "Sverige", "SE"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        country.Name.Should().Be("Sverige");
        country.Code.Should().Be("SE");
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCountry_ThrowsCountryNotFoundException()
    {
        _countriesDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Country?>(null));

        Func<Task> act = async () => await _handler.Handle(
            new UpdateCountryCommand(Guid.NewGuid(), "Sverige", "SE"), CancellationToken.None);

        await act.Should().ThrowAsync<CountryNotFoundException>();
    }
}
