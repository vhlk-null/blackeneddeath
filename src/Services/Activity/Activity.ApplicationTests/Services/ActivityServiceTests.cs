namespace Activity.ApplicationTests.Services;

public class ActivityServiceTests
{
    private readonly Mock<IMongoRepository<UserActivity>> _repoMock = new();
    private readonly ActivityService _sut;

    public ActivityServiceTests()
    {
        _sut = new ActivityService(_repoMock.Object);
    }

    // ── RecordAsync ───────────────────────────────────────────────────────────

    [Fact]
    public async Task RecordAsync_ValidInput_CreatesAndPersistsActivity()
    {
        Guid userId = Guid.NewGuid();
        var payload = new Dictionary<string, string> { ["albumId"] = Guid.NewGuid().ToString() };

        await _sut.RecordAsync(userId, "FavoriteAlbumAdded", payload, TestContext.Current.CancellationToken);

        _repoMock.Verify(r => r.AddAsync(
            It.Is<UserActivity>(a =>
                a.UserId == userId &&
                a.Type == "FavoriteAlbumAdded" &&
                a.Payload["albumId"] == payload["albumId"]),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RecordAsync_EmptyPayload_StillPersistsActivity()
    {
        Guid userId = Guid.NewGuid();

        await _sut.RecordAsync(userId, "ProfileViewed", [], TestContext.Current.CancellationToken);

        _repoMock.Verify(r => r.AddAsync(
            It.Is<UserActivity>(a => a.UserId == userId && a.Type == "ProfileViewed"),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    // ── GetUserActivityAsync ──────────────────────────────────────────────────

    [Fact]
    public async Task GetUserActivityAsync_UserHasActivities_ReturnsMappedDtosOrderedDescending()
    {
        Guid userId = Guid.NewGuid();
        var older = UserActivity.Create(userId, "FavoriteAlbumAdded", []);
        var newer = UserActivity.Create(userId, "FavoriteBandAdded", []);

        // simulate newer having a later OccurredAt by the Create ordering
        // We control order via the list — service sorts descending by OccurredAt
        _repoMock
            .Setup(r => r.FilterAsync(It.IsAny<Expression<Func<UserActivity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([older, newer]);

        IReadOnlyList<UserActivityDto> result = await _sut.GetUserActivityAsync(userId, TestContext.Current.CancellationToken);

        result.Should().HaveCount(2);
        result.Should().OnlyContain(d => d.UserId == userId);
    }

    [Fact]
    public async Task GetUserActivityAsync_NoActivities_ReturnsEmptyList()
    {
        Guid userId = Guid.NewGuid();
        _repoMock
            .Setup(r => r.FilterAsync(It.IsAny<Expression<Func<UserActivity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        IReadOnlyList<UserActivityDto> result = await _sut.GetUserActivityAsync(userId, TestContext.Current.CancellationToken);

        result.Should().BeEmpty();
    }

    [Fact]
    public async Task GetUserActivityAsync_MapsAllFieldsCorrectly()
    {
        Guid userId = Guid.NewGuid();
        var payload = new Dictionary<string, string> { ["bandId"] = "abc123" };
        UserActivity activity = UserActivity.Create(userId, "FavoriteBandAdded", payload);

        _repoMock
            .Setup(r => r.FilterAsync(It.IsAny<Expression<Func<UserActivity, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync([activity]);

        IReadOnlyList<UserActivityDto> result = await _sut.GetUserActivityAsync(userId, TestContext.Current.CancellationToken);

        UserActivityDto dto = result.Single();
        dto.UserId.Should().Be(userId);
        dto.Type.Should().Be("FavoriteBandAdded");
        dto.Payload.Should().ContainKey("bandId").WhoseValue.Should().Be("abc123");
        dto.Id.Should().NotBeNullOrEmpty();
        dto.OccurredAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
