using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using UserContent.Application.Abstractions;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Repositories;
using Xunit;

namespace UserContent.InfrastructureTests.Repositories;

public class CachedUserContentRepositoryTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    private readonly Mock<IUserContentRepository> _innerMock = new();
    private readonly Mock<IDistributedCache> _cacheMock = new();
    private readonly Mock<IConnectionMultiplexer> _redisMock = new();
    private readonly Mock<IServer> _serverMock = new();
    private readonly Mock<IDatabase> _databaseMock = new();
    private readonly Mock<ILogger<CachedUserContentRepository>> _loggerMock = new();
    private readonly CachedUserContentRepository _sut;

    public CachedUserContentRepositoryTests()
    {
        _redisMock.Setup(x => x.GetServers()).Returns([_serverMock.Object]);
        _redisMock.Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>())).Returns(_databaseMock.Object);
        _serverMock.Setup(x => x.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns([]);

        _sut = new CachedUserContentRepository(_innerMock.Object, _cacheMock.Object, _redisMock.Object, _loggerMock.Object);
    }

    // ── GetUserProfileWithDetailsAsync ────────────────────────────────────────

    [Fact]
    public async Task GetUserProfileWithDetailsAsync_CacheHit_ReturnsCachedValueWithoutCallingInner()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "user", Email = "u@u.com", RegisteredDate = DateTime.UtcNow };
        var cacheKey = $"{userId}:UserProfileInfo:GetUserProfileWithDetailsAsync";
        SetupCacheHit(cacheKey, profile);

        var result = await _sut.GetUserProfileWithDetailsAsync(userId);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
        _innerMock.Verify(x => x.GetUserProfileWithDetailsAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserProfileWithDetailsAsync_CacheMiss_CallsInnerAndStoresInCache()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "user", Email = "u@u.com", RegisteredDate = DateTime.UtcNow };
        SetupCacheMiss();
        _innerMock.Setup(x => x.GetUserProfileWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetUserProfileWithDetailsAsync(userId);

        result.Should().Be(profile);
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetUserProfileWithDetailsAsync_CacheMissAndInnerReturnsNull_DoesNotWriteToCache()
    {
        var userId = Guid.NewGuid();
        SetupCacheMiss();
        _innerMock.Setup(x => x.GetUserProfileWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserProfileInfo?)null);

        var result = await _sut.GetUserProfileWithDetailsAsync(userId);

        result.Should().BeNull();
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetUserProfileWithDetailsAsync_CacheReadThrows_FallsBackToInnerRepo()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "user", Email = "u@u.com", RegisteredDate = DateTime.UtcNow };
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Redis down"));
        _innerMock.Setup(x => x.GetUserProfileWithDetailsAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetUserProfileWithDetailsAsync(userId);

        result.Should().Be(profile);
    }

    // ── GetAlbumAsync ─────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAlbumAsync_CacheHit_ReturnsCachedValueWithoutCallingInner()
    {
        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Title = "Symbolic" };
        var cacheKey = $"{albumId}:Album:GetAlbumAsync";
        SetupCacheHit(cacheKey, album);

        var result = await _sut.GetAlbumAsync(albumId);

        result.Should().NotBeNull();
        result!.Id.Should().Be(albumId);
        _innerMock.Verify(x => x.GetAlbumAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task GetAlbumAsync_CacheMiss_CallsInnerAndStoresInCache()
    {
        var albumId = Guid.NewGuid();
        var album = new Album { Id = albumId, Title = "Symbolic" };
        SetupCacheMiss();
        _innerMock.Setup(x => x.GetAlbumAsync(albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        var result = await _sut.GetAlbumAsync(albumId);

        result.Should().Be(album);
        _cacheMock.Verify(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── Pass-through methods ──────────────────────────────────────────────────

    [Fact]
    public async Task GetFavoriteAlbumAsync_DelegatesToInnerRepo()
    {
        var userId = Guid.NewGuid();
        var albumId = Guid.NewGuid();
        var fa = new FavoriteAlbum { UserId = userId, AlbumId = albumId };
        _innerMock.Setup(x => x.GetFavoriteAlbumAsync(userId, albumId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fa);

        var result = await _sut.GetFavoriteAlbumAsync(userId, albumId);

        result.Should().Be(fa);
    }

    [Fact]
    public async Task GetFavoriteBandAsync_DelegatesToInnerRepo()
    {
        var userId = Guid.NewGuid();
        var bandId = Guid.NewGuid();
        var fb = new FavoriteBand { UserId = userId, BandId = bandId };
        _innerMock.Setup(x => x.GetFavoriteBandAsync(userId, bandId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(fb);

        var result = await _sut.GetFavoriteBandAsync(userId, bandId);

        result.Should().Be(fb);
    }

    [Fact]
    public async Task SaveChangesAsync_DelegatesToInnerRepo()
    {
        _innerMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(3);

        var result = await _sut.SaveChangesAsync();

        result.Should().Be(3);
    }

    // ── Cache invalidation ────────────────────────────────────────────────────

    [Fact]
    public async Task AddAsync_CallsInnerAndTriggersInvalidation()
    {
        var fa = new FavoriteAlbum { UserId = Guid.NewGuid(), AlbumId = Guid.NewGuid() };

        await _sut.AddAsync(fa);

        _innerMock.Verify(x => x.AddAsync(fa, It.IsAny<CancellationToken>()), Times.Once);
        _redisMock.Verify(x => x.GetServers(), Times.Once);
    }

    [Fact]
    public void Remove_CallsInnerAndTriggersInvalidation()
    {
        var fa = new FavoriteAlbum { UserId = Guid.NewGuid(), AlbumId = Guid.NewGuid() };

        _sut.Remove(fa);

        _innerMock.Verify(x => x.Remove(fa), Times.Once);
        _redisMock.Verify(x => x.GetServers(), Times.Once);
    }

    [Fact]
    public async Task AddAsync_WhenKeysExist_DeletesFromRedis()
    {
        var fa = new FavoriteAlbum { UserId = Guid.NewGuid(), AlbumId = Guid.NewGuid() };
        var existingKey = new RedisKey("some:cache:key");
        _serverMock.Setup(x => x.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns([existingKey]);
        _databaseMock.Setup(x => x.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(1);

        await _sut.AddAsync(fa);

        _databaseMock.Verify(x => x.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()), Times.Once);
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private void SetupCacheHit<T>(string key, T value)
    {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(value, JsonOptions);
        _cacheMock.Setup(x => x.GetAsync(key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);
    }

    private void SetupCacheMiss()
    {
        _cacheMock.Setup(x => x.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);
        _cacheMock.Setup(x => x.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
    }
}
