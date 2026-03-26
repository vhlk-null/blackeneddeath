using FluentAssertions;
using Library.Application.Data;
using Library.Application.Services.Countries.Commands.CreateCountry;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Countries;

public class CreateCountryHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Country>> _countriesDbSetMock;
    private readonly CreateCountryCommandHandler _handler;

    public CreateCountryHandlerTests()
    {
        _countriesDbSetMock = MockDbSetFactory.Create<Country>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Countries).Returns(_countriesDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateCountryCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsCountryAndReturnsId()
    {
        var command = new CreateCountryCommand("Norway", "NO");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _countriesDbSetMock.Verify(x => x.Add(It.IsAny<Country>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesCountryWithCorrectValues()
    {
        var command = new CreateCountryCommand("Sweden", "SE");

        await _handler.Handle(command, CancellationToken.None);

        _countriesDbSetMock.Verify(
            x => x.Add(It.Is<Country>(c => c.Name == "Sweden" && c.Code == "SE")),
            Times.Once);
    }
}
