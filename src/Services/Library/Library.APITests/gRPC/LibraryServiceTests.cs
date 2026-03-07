using System.Linq.Expressions;
using BuildingBlocks.Repositories;
using FluentAssertions;
using Grpc.Core;
using Library.API.gRPC.Services;
using Library.API.Mappings;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Library.Grpc;
using Library.Infrastructure.Data;
using Moq;
using Xunit;

namespace Library.APITests.gRPC;

public class LibraryServiceTests
{
    private readonly Mock<IRepository<LibraryContext>> _repoMock = new();
    private readonly LibraryService _sut;
    private readonly ServerCallContext _callContext = new Mock<ServerCallContext>().Object;

    static LibraryServiceTests() => MappingConfig.RegisterMappings();

    public LibraryServiceTests()
    {
        _sut = new LibraryService(_repoMock.Object);
    }

    // ── GetAlbumById ──────────────────────────────────────────────────────────

    [Fact]
    public async Task GetAlbumById_ValidId_ReturnsMappedResponse()
    {
        var albumId = Guid.NewGuid();
        var album = Album.Create(
            "Symbolic", AlbumType.FullLength,
            AlbumRelease.Of(1995, AlbumFormat.CD),
            "cover.jpg", null, AlbumId.Of(albumId));
        _repoMock
            .Setup(r => r.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        var response = await _sut.GetAlbumById(new GetAlbumRequest { Id = albumId.ToString() }, _callContext);

        response.Id.Should().Be(albumId.ToString());
        response.Title.Should().Be("Symbolic");
        response.ReleaseDate.Should().Be(1995);
        response.CoverUrl.Should().Be("cover.jpg");
    }

    [Fact]
    public async Task GetAlbumById_InvalidGuid_ThrowsRpcExceptionWithInvalidArgument()
    {
        var act = async () => await _sut.GetAlbumById(new GetAlbumRequest { Id = "not-a-guid" }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task GetAlbumById_EmptyId_ThrowsRpcExceptionWithInvalidArgument()
    {
        var act = async () => await _sut.GetAlbumById(new GetAlbumRequest { Id = "" }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task GetAlbumById_AlbumNotFound_ThrowsRpcExceptionWithNotFound()
    {
        var albumId = Guid.NewGuid();
        _repoMock
            .Setup(r => r.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);

        var act = async () => await _sut.GetAlbumById(new GetAlbumRequest { Id = albumId.ToString() }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.NotFound);
    }

    // ── GetBandById ───────────────────────────────────────────────────────────

    [Fact]
    public async Task GetBandById_ValidId_ReturnsMappedResponse()
    {
        var bandId = Guid.NewGuid();
        var band = Band.Create(
            "Death", null, "logo.png",
            BandActivity.Of(1984, null),
            BandStatus.Active, BandId.Of(bandId));
        _repoMock
            .Setup(r => r.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(band);

        var response = await _sut.GetBandById(new GetBandRequest { Id = bandId.ToString() }, _callContext);

        response.Id.Should().Be(bandId.ToString());
        response.Title.Should().Be("Death");
        response.ReleaseDate.Should().Be(1984);
        response.LogoUrl.Should().Be("logo.png");
    }

    [Fact]
    public async Task GetBandById_InvalidGuid_ThrowsRpcExceptionWithInvalidArgument()
    {
        var act = async () => await _sut.GetBandById(new GetBandRequest { Id = "not-a-guid" }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task GetBandById_EmptyId_ThrowsRpcExceptionWithInvalidArgument()
    {
        var act = async () => await _sut.GetBandById(new GetBandRequest { Id = "" }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.InvalidArgument);
    }

    [Fact]
    public async Task GetBandById_BandNotFound_ThrowsRpcExceptionWithNotFound()
    {
        var bandId = Guid.NewGuid();
        _repoMock
            .Setup(r => r.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Band?)null);

        var act = async () => await _sut.GetBandById(new GetBandRequest { Id = bandId.ToString() }, _callContext);

        var ex = await act.Should().ThrowAsync<RpcException>();
        ex.Which.StatusCode.Should().Be(StatusCode.NotFound);
    }
}
