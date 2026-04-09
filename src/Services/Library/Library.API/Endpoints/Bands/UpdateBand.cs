namespace Library.API.Endpoints.Bands;

public record UpdateBandRequest
{
    public string Band { get; init; } = string.Empty;
    public IFormFile? LogoUrl { get; init; }
}

public record UpdateBandResponse(bool IsSuccess);

public class UpdateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/bands/{id:guid}",
                async ([FromRoute] Guid id, [FromForm] UpdateBandRequest request, ISender sender) =>
                {
                    UpdateBandDto? bandDto = JsonSerializer.Deserialize<UpdateBandDto>(request.Band,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                    if (bandDto is null)
                        return Results.Problem("Could not deserialize band data.", instance: "/bands", statusCode: StatusCodes.Status400BadRequest);

                    bandDto = bandDto with { Id = id };

                    Stream? logoStream = null;
                    if (request.LogoUrl is not null)
                    {
                        logoStream = new MemoryStream();
                        await request.LogoUrl.CopyToAsync(logoStream);
                        logoStream.Position = 0;
                    }

                    UpdateBandCommand command = new UpdateBandCommand(
                        bandDto,
                        logoStream,
                        request.LogoUrl?.ContentType,
                        request.LogoUrl?.FileName);

                    UpdateBandResult result = await sender.Send(command);
                    return Results.Ok(new UpdateBandResponse(result.IsSuccess));
                })
            .WithName("UpdateBand")
            .Accepts<UpdateBandRequest>("multipart/form-data")
            .Produces<UpdateBandResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Band")
            .WithDescription("Update Band")
            .WithTags("Bands")
            .DisableAntiforgery()
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly");
    }
}
