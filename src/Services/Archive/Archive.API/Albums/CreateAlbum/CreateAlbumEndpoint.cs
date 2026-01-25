using BuildingBlocks.Resources.ResourceManagement;

namespace Archive.API.Albums.CreateAlbum
{
    public record CreateAlbumRequest(
    string Title,
    int ReleaseDate,
    AlbumType Type,
    AlbumFormat Format,
    string? Label,
    Guid? CountryId,
    List<Guid> BandIds,
    List<Guid> GenreIds,
    List<Guid> TagIds);

    public record CreateAlbumResponse(Guid Id);

    public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumRequest>
    {
        public CreateAlbumCommandValidator()
        {
            RuleFor(x => x.Title)
             .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
             .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.Label)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
             .MaximumLength(200).WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.CountryId)
                .NotEmpty().WithMessage(ValidationMessages.EmptyRequiredField)
                .WithMessage(ValidationMessages.MaxLengthIsExceeded);

            RuleFor(x => x.ReleaseDate)
                .GreaterThan(0).WithMessage(ValidationMessages.ReleaseYearRequired)
                .GreaterThan(1900).WithMessage(ValidationMessages.ReleaseYearTooOld)
                .LessThanOrEqualTo(DateTime.UtcNow.Year).WithMessage(ValidationMessages.ReleaseYearInFuture);
        }
    }

    public class CreateAlbumEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/albums",
                async (CreateAlbumRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateAlbumCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateAlbumResponse>();

                return Results.Created($"/albums/{response.Id}", response);
            })
            .WithName("CreatedAlbum")
            .Produces<CreateAlbumResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Create Album")
            .WithDescription("Create Album");
        }
    }
}
