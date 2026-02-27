using Library.Domain.Enums;

namespace Library.Application.Albums.Queries.GetAlbums;

public record GetAlbumsRequest(int? PageNumber, int? PageSize = 10);

public record GetAlbumsResult(
    List<AlbumDto> Items,
    int PageNumber,
    int PageSize,
    int TotalCount,
    int TotalPages,
    bool HasPreviousPage,
    bool HasNextPage
);

public record AlbumDto
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public int ReleaseDate { get; init; }
    public string? CoverUrl { get; init; }
    public AlbumType Type { get; init; }
    public AlbumFormat Format { get; init; }
    public string? Label { get; init; }

    public List<BandDto> Bands { get; init; } = new();
    public List<CountryDto> Countries { get; init; } = new();
    public List<string> StreamingLinks { get; init; } = new();
    public List<TrackDto> Tracks { get; init; } = new();
    public List<GenreDto> Genres { get; init; } = new();
}

public record BandDto(Guid Id, string Name, string? LogoUrl);
public record CountryDto(Guid Id, string Name, string Code);
public record TrackDto(Guid Id, string Title);
public record GenreDto(Guid Id, string Name);

public class GetAlbumsEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/albums", async ([AsParameters] GetAlbumsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetAlbumsQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetAlbumsResult>();
                return Results.Ok(response);
            })
            .WithName("GetAlbums")
            .Produces<GetAlbumsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Albums")
            .WithDescription("Get Albums");
    }
}