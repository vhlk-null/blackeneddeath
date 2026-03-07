using FluentAssertions;
using Library.API.Mappings;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Library.Grpc;
using Mapster;
using Xunit;

namespace Library.APITests.Mappings;

public class GrpcMappingTests
{
    static GrpcMappingTests() => MappingConfig.RegisterMappings();

    [Fact]
    public void Album_AdaptsTo_GetAlbumResponse_IdIsParsableGuid()
    {
        var albumId = Guid.NewGuid();
        var album = Album.Create(
            "Symbolic", AlbumType.FullLength,
            AlbumRelease.Of(1995, AlbumFormat.CD),
            "cover.jpg", null, AlbumId.Of(albumId));

        var response = album.Adapt<GetAlbumResponse>();

        Guid.TryParse(response.Id, out var parsed).Should().BeTrue("Id must be a plain GUID string, not AlbumId.ToString()");
        parsed.Should().Be(albumId);
        response.Title.Should().Be("Symbolic");
        response.ReleaseDate.Should().Be(1995);
        response.CoverUrl.Should().Be("cover.jpg");
    }

    [Fact]
    public void Band_AdaptsTo_GetBandResponse_IdIsParsableGuid()
    {
        var bandId = Guid.NewGuid();
        var band = Band.Create(
            "Death", null, "logo.png",
            BandActivity.Of(1984, null),
            BandStatus.Active, BandId.Of(bandId));

        var response = band.Adapt<GetBandResponse>();

        Guid.TryParse(response.Id, out var parsed).Should().BeTrue("Id must be a plain GUID string, not BandId.ToString()");
        parsed.Should().Be(bandId);
        response.Title.Should().Be("Death");
        response.ReleaseDate.Should().Be(1984);
        response.LogoUrl.Should().Be("logo.png");
    }

    [Fact]
    public void Album_AdaptsTo_GetAlbumResponse_NullCoverUrl_MapsAsEmptyString()
    {
        var album = Album.Create(
            "Human", AlbumType.FullLength,
            AlbumRelease.Of(1991, AlbumFormat.CD),
            null, null);

        var response = album.Adapt<GetAlbumResponse>();

        response.CoverUrl.Should().BeEmpty();
    }

    [Fact]
    public void Band_AdaptsTo_GetBandResponse_NullFormedYear_MapsAsZero()
    {
        var band = Band.Create(
            "Unknown Band", null, null,
            BandActivity.Of(null, null),
            BandStatus.Active);

        var response = band.Adapt<GetBandResponse>();

        response.ReleaseDate.Should().Be(0);
        response.LogoUrl.Should().BeEmpty();
    }
}
