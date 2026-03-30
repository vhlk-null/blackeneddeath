using System.Text.Json;
using Library.Application.Services.Bands.Commands.UpdateBand;
using Microsoft.AspNetCore.Mvc;

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
        app.MapPut("/bands",
                async ([FromForm] UpdateBandRequest request, ISender sender) =>
                {
                    var bandDto = JsonSerializer.Deserialize<UpdateBandDto>(request.Band,
                        new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true,
                            Converters = { new JsonStringEnumConverter() }
                        });

                    if (bandDto is null)
                        return Results.Problem("Could not deserialize band data.", instance: "/bands", statusCode: StatusCodes.Status400BadRequest);

                    var command = new UpdateBandCommand(
                        bandDto,
                        request.LogoUrl?.OpenReadStream(),
                        request.LogoUrl?.ContentType,
                        request.LogoUrl?.FileName);

                    var result = await sender.Send(command);
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
            .DisableAntiforgery();
    }
}
