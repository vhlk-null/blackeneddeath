using FluentAssertions;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.Application.Exceptions;
using Library.Application.Services.Bands.Commands.CreateBand;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;
using Xunit;

namespace Library.ApplicationTests.Bands;

public class CreateBandHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Band>> _bandsDbSetMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
    private readonly CreateBandCommandHandler _handler;

    public CreateBandHandlerTests()
    {
        _bandsDbSetMock = MockDbSetFactory.Create<Band>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Bands).Returns(_bandsDbSetMock.Object);
        _contextMock.Setup(x => x.Countries).Returns(MockDbSetFactory.Create<Country>().Object);
        _contextMock.Setup(x => x.Genres).Returns(MockDbSetFactory.Create<Genre>().Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _storageMock = new Mock<IStorageService>();

        DefaultHttpContext httpContext = new();
        httpContext.User = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        _handler = new CreateBandCommandHandler(_contextMock.Object, _storageMock.Object, _httpContextAccessorMock.Object);
    }

    private static CreateBandDto SimpleDto(string name = "Death") =>
        new(name, null, 1983, null, BandStatus.Active, [], []);

    [Fact]
    public async Task Handle_ValidCommand_AddsBandAndReturnsId()
    {
        CreateBandResult result = await _handler.Handle(new CreateBandCommand(SimpleDto()), CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _bandsDbSetMock.Verify(x => x.Add(It.IsAny<Band>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithCountry_AddsBandWithCountry()
    {
        Guid countryId = Guid.NewGuid();
        Country country = Country.Create(CountryId.Of(countryId), "USA", "US");
        _contextMock.Setup(x => x.Countries).Returns(MockDbSetFactory.Create(country).Object);

        CreateBandCommand command = new(new CreateBandDto(
            "Death", null, 1983, null, BandStatus.Active, [countryId], []));

        CreateBandResult result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _bandsDbSetMock.Verify(x => x.Add(It.Is<Band>(b => b.BandCountries.Any())), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithGenre_AddsBandWithGenre()
    {
        Guid genreId = Guid.NewGuid();
        Genre genre = Genre.Create(GenreId.Of(genreId), "Death Metal");
        _contextMock.Setup(x => x.Genres).Returns(MockDbSetFactory.Create(genre).Object);

        CreateBandCommand command = new(new CreateBandDto(
            "Death", null, 1983, null, BandStatus.Active, [], [genreId]));

        CreateBandResult result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _bandsDbSetMock.Verify(x => x.Add(It.Is<Band>(b => b.BandGenres.Any())), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidCountryId_ThrowsCountryNotFoundException()
    {
        CreateBandCommand command = new(new CreateBandDto(
            "Death", null, 1983, null, BandStatus.Active, [Guid.NewGuid()], []));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<CountryNotFoundException>();
    }

    [Fact]
    public async Task Handle_InvalidGenreId_ThrowsGenreNotFoundException()
    {
        CreateBandCommand command = new(new CreateBandDto(
            "Death", null, 1983, null, BandStatus.Active, [], [Guid.NewGuid()]));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<GenreNotFoundException>();
    }

    [Fact]
    public async Task Handle_WithLogoFile_UploadsLogoAndStoresKey()
    {
        const string logoKey = "bands/death/logo/abc.jpg";
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(logoKey);

        CreateBandCommand command = new(SimpleDto(), Stream.Null, "image/jpeg", "logo.jpg");

        await _handler.Handle(command, CancellationToken.None);

        _storageMock.Verify(x => x.UploadFileAsync(
            "bands/death/logo", It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
        _bandsDbSetMock.Verify(x => x.Add(It.Is<Band>(b => b.LogoUrl == logoKey)), Times.Once);
    }

    [Fact]
    public async Task Handle_AdminUser_ApprovesBand()
    {
        DefaultHttpContext adminContext = new();
        adminContext.User = new ClaimsPrincipal(new ClaimsIdentity(
            [new Claim(ClaimTypes.Role, "admin")], "test"));
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(adminContext);

        await _handler.Handle(new CreateBandCommand(SimpleDto()), CancellationToken.None);

        _bandsDbSetMock.Verify(x => x.Add(It.Is<Band>(b => b.IsApproved)), Times.Once);
    }
}
