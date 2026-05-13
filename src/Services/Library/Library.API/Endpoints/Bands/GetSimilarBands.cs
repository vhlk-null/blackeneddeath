namespace Library.API.Endpoints.Bands;

public record GetSimilarBandsResponse(PaginatedResult<BandCardDto> SimilarBands);

public class GetSimilarBands : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/bands/slug/{slug}/similar", async (string slug, ISender sender, int pageNumber = 1, int pageSize = 4) =>
            {
                GetSimilarBandsQuery query = new(slug, PageNumber: pageNumber, PageSize: pageSize);
                GetSimilarBandsResult result = await sender.Send(query);
                return Results.Ok(new GetSimilarBandsResponse(result.SimilarBands));
            })
            .WithName("GetSimilarBands")
            .Produces<GetSimilarBandsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithTags("Bands");
    }
}
