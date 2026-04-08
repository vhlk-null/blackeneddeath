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
            string folder = $"genres/{Slugify(command.Name)}";
            string extension = Path.GetExtension(command.CoverImageFileName);
            coverKey = await storage.UploadFileAsync(folder, $"{Guid.NewGuid()}{extension}", command.CoverImage, command.CoverImageContentType, cancellationToken);
        }

        GenreCard card = GenreCard.Create(GenreCardId.Of(Guid.NewGuid()), command.Name, command.Description, coverKey, command.OrderNumber);

        context.GenreCards.Add(card);
        await context.SaveChangesAsync(cancellationToken);

        return new CreateGenreCardResult(card.Id.Value);
    }

    private static string Slugify(string value) =>
        Regex.Replace(value.ToLowerInvariant().Trim(), @"[^a-z0-9]+", "-").Trim('-');
}
