namespace BuildingBlocks.Messaging.Events;

public record AlbumBandInfo(Guid Id, string Name, string? Slug);
public record AlbumGenreInfo(Guid Id, string Name, string? Slug);
public record AlbumCountryInfo(Guid Id, string Name, string? Code);
