using System.Text.RegularExpressions;

namespace Library.Application.Services.GenreCards.Commands.UpdateGenreCardCover;

public class UpdateGenreCardCoverCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateGenreCardCoverCommand, UpdateGenreCardCoverResult>
{
    public async ValueTask<UpdateGenreCardCoverResult> Handle(UpdateGenreCardCoverCommand command, CancellationToken cancellationToken)
    {
        var card = await context.GenreCards.FindAsync([GenreCardId.Of(command.GenreCardId)], cancellationToken)
            ?? throw new GenreCardNotFoundException(command.GenreCardId);

        if (card.CoverUrl is not null)
            await storage.DeleteFileAsync(card.CoverUrl, cancellationToken);

        var folder = $"genres/{Slugify(card.Name)}";
        var extension = Path.GetExtension(command.CoverImageFileName);
        var coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);

        card.Update(card.Name, card.Description, coverKey, card.OrderNumber);
        await context.SaveChangesAsync(cancellationToken);

        return new UpdateGenreCardCoverResult(true);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
