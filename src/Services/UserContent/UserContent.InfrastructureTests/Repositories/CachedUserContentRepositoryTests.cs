using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BuildingBlocks.Repositories;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Moq;
using StackExchange.Redis;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Data;
using UserContent.Infrastructure.Repositories;
using Xunit;

namespace UserContent.InfrastructureTests.Repositories;

public class CachedUserContentRepositoryTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };

    private readonly Mock<IRepository<UserContentContext>> _innerMock = new();
    private readonly Mock<IDistributedCache> _cacheMock = new();
    private readonly Mock<IConnectionMultiplexer> _redisMock = new();
    private readonly Mock<IServer> _serverMock = new();
    private readonly Mock<IDatabase> _dbMock = new();
    private readonly UserContentContext _context;
    private readonly CachedUserContentRepository _sut;

    public CachedUserContentRepositoryTests()
    {
        _context = new UserContentContext(
            new DbContextOptionsBuilder<UserContentContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options);

        _innerMock.Setup(r => r.Context).Returns(_context);

        _serverMock
            .Setup(s => s.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns([]);

        _dbMock
            .Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()))
            .ReturnsAsync(0);

        _redisMock.Setup(r => r.GetServers()).Returns([_serverMock.Object]);
        _redisMock.Setup(r => r.GetDatabase(It.IsAny<int>(), It.IsAny<object?>())).Returns(_dbMock.Object);

        _sut = new CachedUserContentRepository(
            _innerMock.Object,
            _cacheMock.Object,
            _redisMock.Object,
            Mock.Of<ILogger<CachedUserContentRepository>>());
    }

    private void SetupCacheMiss() =>
        _cacheMock
            .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

    private void SetupCacheHit<T>(T value) where T : class
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value, JsonOptions));
        _cacheMock
            .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(bytes);
    }

    private void SetupCacheSet() =>
        _cacheMock
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

    // ── Cache miss ─────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetWithIncludesAsync_UserProfileInfo_CacheMiss_FetchesFromInnerAndCaches()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "metal_head" };
        SetupCacheMiss();
        SetupCacheSet();
        _innerMock
            .Setup(r => r.GetWithIncludesAsync(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetWithIncludesAsync<UserProfileInfo>(
            u => u.UserId == userId,
            q => q,
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
        _innerMock.Verify(r => r.GetWithIncludesAsync(
            It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
            It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
        _cacheMock.Verify(c => c.SetAsync(
            It.Is<string>(k => k.Contains(userId.ToString())),
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithIncludesAsync_UserProfileInfo_CacheMiss_NullResult_DoesNotWriteToCache()
    {
        var userId = Guid.NewGuid();
        SetupCacheMiss();
        _innerMock
            .Setup(r => r.GetWithIncludesAsync(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserProfileInfo?)null);

        var result = await _sut.GetWithIncludesAsync<UserProfileInfo>(
            u => u.UserId == userId,
            q => q,
            CancellationToken.None);

        result.Should().BeNull();
        _cacheMock.Verify(c => c.SetAsync(
            It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Cache hit ──────────────────────────────────────────────────────────────

    [Fact]
    public async Task GetWithIncludesAsync_UserProfileInfo_CacheHit_ReturnsFromCacheWithoutCallingInner()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "cached_user" };
        SetupCacheHit(profile);

        var result = await _sut.GetWithIncludesAsync<UserProfileInfo>(
            u => u.UserId == userId,
            q => q,
            CancellationToken.None);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
        result.Username.Should().Be("cached_user");
        _innerMock.Verify(r => r.GetWithIncludesAsync(
            It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
            It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
            It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Non-UserProfileInfo passthrough ────────────────────────────────────────

    [Fact]
    public async Task GetWithIncludesAsync_NonUserProfileInfoType_DelegatesToInnerWithoutTouchingCache()
    {
        var album = new Album { Id = Guid.NewGuid(), Title = "Symbolic" };
        _innerMock
            .Setup(r => r.GetWithIncludesAsync(
                It.IsAny<Expression<Func<Album, bool>>>(),
                It.IsAny<Func<IQueryable<Album>, IQueryable<Album>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        var result = await _sut.GetWithIncludesAsync<Album>(
            a => a.Id == album.Id,
            q => q,
            CancellationToken.None);

        result.Should().Be(album);
        _cacheMock.Verify(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _cacheMock.Verify(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    // ── Cache failure resilience ───────────────────────────────────────────────

    [Fact]
    public async Task GetWithIncludesAsync_CacheReadThrows_FallsThroughToInner()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId, Username = "metal_head" };
        _cacheMock
            .Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Redis unavailable"));
        SetupCacheSet();
        _innerMock
            .Setup(r => r.GetWithIncludesAsync(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var act = () => _sut.GetWithIncludesAsync<UserProfileInfo>(u => u.UserId == userId, q => q);

        await act.Should().NotThrowAsync();
        _innerMock.Verify(r => r.GetWithIncludesAsync(
            It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
            It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task GetWithIncludesAsync_CacheWriteThrows_ReturnsResultWithoutThrowing()
    {
        var userId = Guid.NewGuid();
        var profile = new UserProfileInfo { UserId = userId };
        SetupCacheMiss();
        _cacheMock
            .Setup(c => c.SetAsync(It.IsAny<string>(), It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Redis write failed"));
        _innerMock
            .Setup(r => r.GetWithIncludesAsync(
                It.IsAny<Expression<Func<UserProfileInfo, bool>>>(),
                It.IsAny<Func<IQueryable<UserProfileInfo>, IQueryable<UserProfileInfo>>>(),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(profile);

        var result = await _sut.GetWithIncludesAsync<UserProfileInfo>(u => u.UserId == userId, q => q);

        result.Should().NotBeNull();
        result!.UserId.Should().Be(userId);
    }

    // ── Cache invalidation on writes ───────────────────────────────────────────

    [Fact]
    public async Task AddAsync_EntityWithUserId_CallsInnerAndInvalidatesCache()
    {
        var userId = Guid.NewGuid();
        var fa = new FavoriteAlbum { UserId = userId, AlbumId = Guid.NewGuid() };
        var keys = new RedisKey[] { $"UserContent:{userId}:UserProfileInfo" };
        _serverMock
            .Setup(s => s.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns(keys);

        await _sut.AddAsync(fa);

        _innerMock.Verify(r => r.AddAsync(fa, It.IsAny<CancellationToken>()), Times.Once);
        _dbMock.Verify(d => d.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public void Delete_EntityWithUserId_CallsInnerAndInvalidatesCache()
    {
        var userId = Guid.NewGuid();
        var fb = new FavoriteBand { UserId = userId, BandId = Guid.NewGuid() };
        var keys = new RedisKey[] { $"UserContent:{userId}:UserProfileInfo" };
        _serverMock
            .Setup(s => s.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns(keys);

        _sut.Delete(fb);

        _innerMock.Verify(r => r.Delete(fb), Times.Once);
        _dbMock.Verify(d => d.KeyDelete(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()), Times.Once);
    }

    [Fact]
    public async Task AddAsync_CacheInvalidationThrows_DoesNotThrow()
    {
        var fa = new FavoriteAlbum { UserId = Guid.NewGuid(), AlbumId = Guid.NewGuid() };
        _redisMock.Setup(r => r.GetServers()).Throws(new Exception("Redis unavailable"));

        var act = () => _sut.AddAsync(fa);

        await act.Should().NotThrowAsync();
        _innerMock.Verify(r => r.AddAsync(fa, It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── SaveChangesAsync ───────────────────────────────────────────────────────

    [Fact]
    public async Task SaveChangesAsync_NoModifiedAlbums_DelegatesToInnerWithoutInvalidatingCache()
    {
        _innerMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(3);

        var result = await _sut.SaveChangesAsync();

        result.Should().Be(3);
        _redisMock.Verify(r => r.GetServers(), Times.Never);
    }

    [Fact]
    public async Task SaveChangesAsync_WithModifiedAlbum_InvalidatesAllUserProfileCaches()
    {
        var album = new Album { Id = Guid.NewGuid(), Title = "Symbolic" };
        await _context.Albums.AddAsync(album);
        await _context.SaveChangesAsync();

        album.Title = "Symbolic (Updated)";
        _context.ChangeTracker.DetectChanges();

        var keys = new RedisKey[] { "UserContent:user1:UserProfileInfo" };
        _serverMock
            .Setup(s => s.Keys(It.IsAny<int>(), It.IsAny<RedisValue>(), It.IsAny<int>(),
                It.IsAny<long>(), It.IsAny<int>(), It.IsAny<CommandFlags>()))
            .Returns(keys);
        _innerMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _sut.SaveChangesAsync();

        _dbMock.Verify(d => d.KeyDeleteAsync(It.IsAny<RedisKey[]>(), It.IsAny<CommandFlags>()), Times.Once);
    }
}
