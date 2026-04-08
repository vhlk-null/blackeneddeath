using System.Text.RegularExpressions;

namespace Library.Application.Services.GenreCards.Commands.UpdateGenreCard;

public class UpdateGenreCardCommandHandler(ILibraryDbContext context, IStorageService storage)
    : BuildingBlocks.CQRS.ICommandHandler<UpdateGenreCardCommand, UpdateGenreCardResult>
{
    public async ValueTask<UpdateGenreCardResult> Handle(UpdateGenreCardCommand command, CancellationToken cancellationToken)
    {
        GenreCard card = await context.GenreCards
                             .Include(c => c.GenreCardGenres)
                             .Include(c => c.GenreCardTags)
                             .FirstOrDefaultAsync(c => c.Id == GenreCardId.Of(command.Id), cancellationToken)
                         ?? throw new GenreCardNotFoundException(command.Id);

        string? coverKey = card.CoverUrl;

        if (command.CoverImage is not null && command.CoverImageContentType is not null && command.CoverImageFileName is not null)
        {
            if (card.CoverUrl is not null)
                await storage.DeleteFileAsync(card.CoverUrl, cancellationToken);

            string folder = $"genres/{Slugify(command.Name)}";
            string extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        card.Update(command.Name, command.Description, coverKey, command.OrderNumber);

        // Sync genres
        HashSet<GenreId> desiredGenreIds = (command.GenreIds ?? []).Select(GenreId.Of).ToHashSet();
        HashSet<GenreId> currentGenreIds = card.GenreCardGenres.Select(g => g.GenreId).ToHashSet();
        foreach (GenreId id in currentGenreIds.Except(desiredGenreIds)) card.RemoveGenre(id);
        foreach (GenreId id in desiredGenreIds.Except(currentGenreIds))
        {
            bool exists = await context.Genres.AnyAsync(g => g.Id == id, cancellationToken);
            if (exists) card.AddGenre(id);
        }

        // Sync tags
        HashSet<TagId> desiredTagIds = (command.TagIds ?? []).Select(TagId.Of).ToHashSet();
        HashSet<TagId> currentTagIds = card.GenreCardTags.Select(t => t.TagId).ToHashSet();
        foreach (TagId id in currentTagIds.Except(desiredTagIds)) card.RemoveTag(id);
        foreach (TagId id in desiredTagIds.Except(currentTagIds))
        {
            bool exists = await context.Tags.AnyAsync(t => t.Id == id, cancellationToken);
            if (exists) card.AddTag(id);
        }

        await context.SaveChangesAsync(cancellationToken);

        return new UpdateGenreCardResult(true);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
