using Library.Application.Services.Bands.Commands.UpdateBandLogo;
using Microsoft.AspNetCore.Mvc;

namespace Library.API.Endpoints.Bands;

public record UpdateBandLogoRequest
{
    public IFormFile? Logo { get; init; }
    public IFormFile? LogoUrl { get; init; }
}

public record UpdateBandLogoResponse(bool IsSuccess);

public class UpdateBandLogo : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut("/bands/{id:guid}/logo",
                async (Guid id, [FromForm] UpdateBandLogoRequest request, ISender sender) =>
                {
                    var logo = request.Logo ?? request.LogoUrl;
                    if (logo is null)
                        return Results.Problem("Logo file is required.", instance: $"/bands/{id}/logo", statusCode: StatusCodes.Status400BadRequest);

                    var command = new UpdateBandLogoCommand(
                        id,
                        logo.OpenReadStream(),
                        logo.ContentType,
                        logo.FileName);

                    var result = await sender.Send(command);

                    return Results.Ok(result.Adapt<UpdateBandLogoResponse>());
                })
            .WithName("UpdateBandLogo")
            .Accepts<UpdateBandLogoRequest>("multipart/form-data")
            .Produces<UpdateBandLogoResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Update Band Logo")
            .WithDescription("Upload or replace the logo for a band")
            .WithTags("Bands")
            .DisableAntiforgery();
    }
}
