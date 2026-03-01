using BuildingBlocks.Exceptions;
using FluentAssertions;
using Moq;
using UserContent.Application.Abstractions;
using UserContent.Application.Exceptions;
using UserContent.Application.Mappings;
using UserContent.Application.Services;
using UserContent.Domain.Models;
using Xunit;

namespace UserContent.ApplicationTests.Services;

public class UserContentServiceTests
{
    private readonly Mock<IUserContentRepository> _repoMock = new();
    private readonly Mock<ILibraryService> _libraryMock = new();
    private readonly UserContentService _sut;

    static UserContentServiceTests() => MappingConfig.RegisterMappings();

    public UserContentServiceTests()
    {
        _sut = new UserContentService(_repoMock.Object, _libraryMock.Object);
    }

    // ── GetUserProfile ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUserProfileAsync_ProfileExists_ReturnsMappedDto()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo
        {
            UserId = userId,
            Username = "metal_head",
            Email = "user@example.com",
            RegisteredDate = DateTime.UtcNow
        };
        _repoMock.Setup(x => x.GetUserProfileWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetUserProfileAsync(userId);

        result.UserId.Should().Be(userId);
        result.Username.Should().Be("metal_head");
        result.Email.Should().Be("user@example.com");
    }

    [Fact]
    public async Task GetUserProfileAsync_ProfileNotFound_ThrowsUserProfileNotFoundException()
    {
        var userId = Guid.NewGuid();
        _repoMock.Setup(x => x.GetUserProfileWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserProfileInfo?)null);

        await _sut.Invoking(s => s.GetUserProfileAsync(userId))
            .Should().ThrowAsync<UserProfileNotFoundException>();
    }

    // ── AddFavoriteAlbum ──────────────────────────────────────────────────────

    [Fact]
    public async Task AddFavoriteAlbumAsync_AlbumExistsLocally_DoesNotCallLibraryAndReturnsUserId()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Title = "Symbolic" };
        _repoMock.Setup(x => x.GetAlbumAsync(albumId, It.IsAny<CancellationToken>()))
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
        _repoMock.Setup(x => x.GetAlbumAsync(albumId, It.IsAny<CancellationToken>()))
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
        _repoMock.Setup(x => x.GetAlbumAsync(albumId, It.IsAny<CancellationToken>()))
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
        _repoMock.Setup(x => x.GetFavoriteAlbumAsync(userId, albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fa);

        await _sut.DeleteFavoriteAlbumAsync(userId, albumId);

        _repoMock.Verify(x => x.Remove(fa), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFavoriteAlbumAsync_FavoriteNotFound_ThrowsFavoriteAlbumNotFoundException()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        _repoMock.Setup(x => x.GetFavoriteAlbumAsync(userId, albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FavoriteAlbum?)null);

        await _sut.Invoking(s => s.DeleteFavoriteAlbumAsync(userId, albumId))
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
        _repoMock.Setup(x => x.GetFavoriteBandAsync(userId, bandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fb);

        await _sut.DeleteFavoriteBandAsync(userId, bandId);

        _repoMock.Verify(x => x.Remove(fb), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteFavoriteBandAsync_FavoriteNotFound_ThrowsFavoriteBandNotFoundException()
    {
        var userId = Guid.NewGuid();
        var bandId = Guid.NewGuid();
        _repoMock.Setup(x => x.GetFavoriteBandAsync(userId, bandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((FavoriteBand?)null);

        await _sut.Invoking(s => s.DeleteFavoriteBandAsync(userId, bandId))
            .Should().ThrowAsync<FavoriteBandNotFoundException>();
    }
}
