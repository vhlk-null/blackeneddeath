using System.Text.RegularExpressions;

namespace Library.Application.Services.GenreCards.Commands.UpdateGenreCard;

public class UpdateGenreCardCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateGenreCardCommand, UpdateGenreCardResult>
{
    public async ValueTask<UpdateGenreCardResult> Handle(UpdateGenreCardCommand command, CancellationToken cancellationToken)
    {
        var card = await context.GenreCards.FindAsync([GenreCardId.Of(command.Id)], cancellationToken)
            ?? throw new GenreCardNotFoundException(command.Id);

        var coverKey = card.CoverUrl;

        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            if (card.CoverUrl is not null)
                await storage.DeleteFileAsync(card.CoverUrl, cancellationToken);

            var folder = $"genres/{Slugify(command.Name)}";
            var extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        card.Update(command.Name, command.Description, coverKey);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateGenreCardResult(true);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
