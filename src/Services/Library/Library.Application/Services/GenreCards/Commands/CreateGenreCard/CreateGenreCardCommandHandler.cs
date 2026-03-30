using System.Text.RegularExpressions;

namespace Library.Application.Services.GenreCards.Commands.CreateGenreCard;

public class CreateGenreCardCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<CreateGenreCardCommand, CreateGenreCardResult>
{
    public async ValueTask<CreateGenreCardResult> Handle(CreateGenreCardCommand command, CancellationToken cancellationToken)
    {
        string? coverKey = null;
        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            var folder = $"genres/{Slugify(command.Name)}";
            var extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        var card = GenreCard.Create(GenreCardId.Of(Guid.NewGuid()), command.Name, command.Description, coverKey);

        context.GenreCards.Add(card);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateGenreCardResult(card.Id.Value);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
