using Library.Domain.Models;

namespace Library.API.Bands.GetBands
{
    public record GetBandsRequest(int? PageNumber, int? PageSize = 10);

    public record GetBandsResult(
        List<BandDto> Items,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages,
        bool HasPreviousPage,
        bool HasNextPage
    );

    public record BandDto
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string? Bio { get; init; }
        public string? LogoUrl { get; init; }
        public int? FormedYear { get; init; }
        public int? DisbandedYear { get; init; }
        public BandStatus Status { get; init; }

        public CountryDto? Country { get; init; }
        public List<AlbumDto> Albums { get; init; } = new();
        public List<GenreDto> Genres { get; init; } = new();
    }

    public record CountryDto(Guid Id, string Name, string Code);
    public record AlbumDto(Guid Id, string Title, int ReleaseDate);
    public record GenreDto(Guid Id, string Name);

    public class GetBandsEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/bands", async ([AsParameters] GetBandsRequest request, ISender sender) =>
            {
                var query = request.Adapt<GetBandsQuery>();
                var result = await sender.Send(query);
                var response = result.Adapt<GetBandsResult>();
                return Results.Ok(response);
            })
            .WithName("GetBands")
            .Produces<GetBandsResult>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get Bands")
            .WithDescription("Get Bands");
        }
    }
}
