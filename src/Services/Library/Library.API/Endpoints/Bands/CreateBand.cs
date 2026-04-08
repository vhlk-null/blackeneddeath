namespace Library.API.Endpoints.Bands;

public record CreateBandRequest
{
    public string Band { get; init; } = string.Empty;
    public IFormFile? LogoUrl { get; init; }
}
public record CreateBandResponse(Guid Id);

public class CreateBand : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/bands",
                async ([FromForm] CreateBandRequest request, ISender sender) =>
                {
                    CreateBandDto? bandDto = JsonSerializer.Deserialize<CreateBandDto>(request.Band,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                    Stream? logoStream = null;
                    if (request.LogoUrl is not null)
                    {
                        logoStream = new MemoryStream();
                        await request.LogoUrl.CopyToAsync(logoStream);
                        logoStream.Position = 0;
                    }

                    CreateBandCommand command = new CreateBandCommand(
                        bandDto!,
                        logoStream,
                        request.LogoUrl?.ContentType,
                        request.LogoUrl?.FileName);

                    CreateBandResult result = await sender.Send(command);

                    return Results.Created($"/bands/{result.Id}", result.Adapt<CreateBandResponse>());
                })
            .WithName("CreateBand")
            .Accepts<CreateBandRequest>("multipart/form-data")
            .Produces<CreateBandResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Band")
            .WithDescription("Create Band")
            .WithTags("Bands")
            .DisableAntiforgery();
    }
}
