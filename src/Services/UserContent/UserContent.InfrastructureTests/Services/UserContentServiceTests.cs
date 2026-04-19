using System.Linq.Expressions;
using BuildingBlocks.Exceptions;
using BuildingBlocks.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using UserContent.Application.Dtos;
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
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly UserContentService _sut;

    static UserContentServiceTests() => MappingConfig.RegisterMappings();

    public UserContentServiceTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns((HttpContext?)null);
        _sut = new UserContentService(_repoMock.Object, _httpContextAccessorMock.Object, Microsoft.Extensions.Logging.Abstractions.NullLogger<UserContentService>.Instance);
    }

    // ── GetUserProfile ────────────────────────────────────────────────────────

    [Fact]
    public async Task GetUserProfileAsync_ProfileExists_ReturnsMappedDto()
    {
        Guid userId = Guid.NewGuid();
        UserProfileInfo profile = new() { UserId = userId, Username = "metal_head", Email = "user@example.com", RegisteredDate = DateTime.UtcNow };
        _repoMock.Setup(x => x.GetWithIncludesAsync<UserProfileInfo>(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        UserProfileDto result = await _sut.GetUserProfileAsync(userId, TestContext.Current.CancellationToken);

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
    public async Task AddFavoriteAlbumAsync_AlbumExistsLocally_ReturnsUserId()
    {
        Guid userId = Guid.NewGuid();
        Guid albumId = Guid.NewGuid();
        Album album = new() { Id = albumId, Title = "Symbolic" };
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        Guid result = await _sut.AddFavoriteAlbumAsync(userId, albumId, TestContext.Current.CancellationToken);

        result.Should().Be(userId);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteAlbumAsync_AlbumNotFound_ThrowsNotFoundException()
    {
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);

        await _sut.Invoking(s => s.AddFavoriteAlbumAsync(Guid.NewGuid(), Guid.NewGuid()))
            .Should().ThrowAsync<NotFoundException>();
    }

    // ── DeleteFavoriteAlbum ───────────────────────────────────────────────────

    [Fact]
    public async Task DeleteFavoriteAlbumAsync_FavoriteExists_RemovesAndSaves()
    {
        Guid userId = Guid.NewGuid();
        Guid albumId = Guid.NewGuid();
        FavoriteAlbum fa = new() { UserId = userId, AlbumId = albumId };
        _repoMock.Setup(x => x.GetByAsync<FavoriteAlbum>(It.IsAny<Expression<Func<FavoriteAlbum, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fa);

        await _sut.DeleteFavoriteAlbumAsync(userId, albumId, TestContext.Current.CancellationToken);

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
    public async Task AddFavoriteBandAsync_BandExistsLocally_ReturnsUserId()
    {
        Guid userId = Guid.NewGuid();
        Guid bandId = Guid.NewGuid();
        Band band = new() { BandId = bandId, BandName = "Death" };
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(band);

        Guid result = await _sut.AddFavoriteBandAsync(userId, bandId, TestContext.Current.CancellationToken);

        result.Should().Be(userId);
        _repoMock.Verify(x => x.AddAsync(
            It.Is<FavoriteBand>(fb => fb.UserId == userId && fb.BandId == bandId),
            It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFavoriteBandAsync_BandNotFound_ThrowsNotFoundException()
    {
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Band?)null);

        await _sut.Invoking(s => s.AddFavoriteBandAsync(Guid.NewGuid(), Guid.NewGuid()))
            .Should().ThrowAsync<NotFoundException>();
    }

    // ── DeleteFavoriteBand ────────────────────────────────────────────────────

    [Fact]
    public async Task DeleteFavoriteBandAsync_FavoriteExists_RemovesAndSaves()
    {
        Guid userId = Guid.NewGuid();
        Guid bandId = Guid.NewGuid();
        FavoriteBand fb = new() { UserId = userId, BandId = bandId };
        _repoMock.Setup(x => x.GetByAsync<FavoriteBand>(It.IsAny<Expression<Func<FavoriteBand, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(fb);

        await _sut.DeleteFavoriteBandAsync(userId, bandId, TestContext.Current.CancellationToken);

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
