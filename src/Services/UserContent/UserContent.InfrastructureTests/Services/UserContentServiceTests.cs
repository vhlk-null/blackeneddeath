using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repositories;
using FluentAssertions;
using Moq;
using UserContent.Application.Abstractions;
using UserContent.Application.Exceptions;
using UserContent.Application.Mappings;
using UserContent.Application.Services;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Data;
using Xunit;

namespace UserContent.InfrastructureTests.Services;

public class UserContentServiceTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly Mock<ILibraryService> _libraryMock = new();
    private readonly UserContentService _sut;

    static UserContentServiceTests() => MappingConfig.RegisterMappings();

    public UserContentServiceTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new UserContentService(_repoMock.Object, _libraryMock.Object);
    }

    // ── GetUserProfile ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUserProfileAsync_ProfileExists_ReturnsMappedDto()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "metal_head", Email = "user@example.com", RegisteredDate = DateTime.UtcNow };
        _repoMock.Setup(x => x.GetWithIncludesAsync<UserProfileInfo>(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetUserProfileAsync(userId);

        result.UserId.Should().Be(userId);
        result.Username.Should().Be("metal_head");
        result.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task GetUserProfileAsync_ProfileNotFound_ThrowsUserProfileNotFoundException()
    {
        _repoMock.Setup(x => x.GetWithIncludesAsync<UserProfileInfo>(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserProfileInfo?)null);

        await _sut.Invoking(s => s.GetUserProfileAsync(Guid.NewGuid()))
            .Should().ThrowAsync<UserProfileNotFoundException>();
    }

    // ── AddFavoriteAlbum ──────────────────────────────────────────────────────

    [Fact]
    public async Task AddFavoriteAlbumAsync_AlbumExistsLocally_DoesNotCallLibraryAndReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Title = "Symbolic" };
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        var result = await _sut.AddFavoriteAlbumAsync(userId, albumId);

        result.Should().Be(userId);
        _libraryMock.Verify(x => x.GetAlbumByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteAlbumAsync_AlbumNotLocalButFoundInLibrary_FetchesThenAddsAndReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Title = "Symbolic" };
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);
        _libraryMock.Setup(x => x.GetAlbumByIdAsync(albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        var result = await _sut.AddFavoriteAlbumAsync(userId, albumId);

        result.Should().Be(userId);
        _repoMock.Verify(x => x.AddAsync(album, It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteAlbumAsync_AlbumNotFoundAnywhere_ThrowsNotFoundException()
    {
        var albumId = Guid.NewGuid();
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);
        _libraryMock.Setup(x => x.GetAlbumByIdAsync(albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);

        await _sut.Invoking(s => s.AddFavoriteAlbumAsync(Guid.NewGuid(), albumId))
            .Should().ThrowAsync<NotFoundException>();
    }

    // ── DeleteFavoriteAlbum ───────────────────────────────────────────────────

    [Fact]
    public async Task DeleteFavoriteAlbumAsync_FavoriteExists_RemovesAndSaves()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        var fa = new FavoriteAlbum { UserId = userId, AlbumId = albumId };
        _repoMock.Setup(x => x.GetByAsync<FavoriteAlbum>(It.IsAny<Expression<Func<FavoriteAlbum, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fa);

        await _sut.DeleteFavoriteAlbumAsync(userId, albumId);

        _repoMock.Verify(x => x.Delete(fa), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFavoriteAlbumAsync_FavoriteNotFound_ThrowsFavoriteAlbumNotFoundException()
    {
        _repoMock.Setup(x => x.GetByAsync<FavoriteAlbum>(It.IsAny<Expression<Func<FavoriteAlbum, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FavoriteAlbum?)null);

        await _sut.Invoking(s => s.DeleteFavoriteAlbumAsync(Guid.NewGuid(), Guid.NewGuid()))
            .Should().ThrowAsync<FavoriteAlbumNotFoundException>();
    }

    // ── AddFavoriteBand ───────────────────────────────────────────────────────

    [Fact]
    public async Task AddFavoriteBandAsync_AddsAndReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var bandId = Guid.NewGuid();

        var result = await _sut.AddFavoriteBandAsync(userId, bandId);

        result.Should().Be(userId);
        _repoMock.Verify(x => x.AddAsync(
            It.Is<FavoriteBand>(fb => fb.UserId == userId && fb.BandId == bandId),
            It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── DeleteFavoriteBand ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteFavoriteBandAsync_FavoriteExists_RemovesAndSaves()
    {
        var userId = Guid.NewGuid();
        var bandId = Guid.NewGuid();
        var fb = new FavoriteBand { UserId = userId, BandId = bandId };
        _repoMock.Setup(x => x.GetByAsync<FavoriteBand>(It.IsAny<Expression<Func<FavoriteBand, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fb);

        await _sut.DeleteFavoriteBandAsync(userId, bandId);

        _repoMock.Verify(x => x.Delete(fb), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFavoriteBandAsync_FavoriteNotFound_ThrowsFavoriteBandNotFoundException()
    {
        _repoMock.Setup(x => x.GetByAsync<FavoriteBand>(It.IsAny<Expression<Func<FavoriteBand, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((FavoriteBand?)null);

        await _sut.Invoking(s => s.DeleteFavoriteBandAsync(Guid.NewGuid(), Guid.NewGuid()))
            .Should().ThrowAsync<FavoriteBandNotFoundException>();
    }
}
