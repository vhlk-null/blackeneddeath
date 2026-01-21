using Archive.API.Data;
using Archive.API.Models;
using Archive.API.Models.JoinTables;
using BuildingBlocks.CQRS;
using BuildingBlocks.Repositories;

namespace Archive.API.Albums.CreateAlbum
{
    public record CreateAlbumCommand(
        string Title,
        DateTime ReleaseDate,
        AlbumType Type,
        AlbumFormat Format,
        string? Label,
        Guid? CountryId,
        List<Guid> BandIds,
        List<Guid> GenreIds,
        List<Guid> TagIds,
        IFormFile? Cover
    ) : ICommand<CreateAlbumResult>;

    public record CreateAlbumResult(Guid Id);

    internal class CreateAlbumCommandHandler(IRepository<ArchiveContext> repo) : ICommandHandler<CreateAlbumCommand, CreateAlbumResult>
    {
        private readonly IRepository<ArchiveContext> repo = repo;

        public async Task<CreateAlbumResult> Handle(CreateAlbumCommand command, CancellationToken cancellationToken)
        {
            var album = new Album
            {
                Id = Guid.NewGuid(),
                Title = command.Title,
                ReleaseDate = command.ReleaseDate,
                Type = command.Type,
                Format = command.Format,
                Label = command.Label,
                //CountryId = command.CountryId,
                //CoverUrl = coverUrl,
                Genres = command.GenreIds.Select(genreId => new AlbumGenre
                {
                    GenreId = genreId
                }).ToList()
            };

            await repo.AddAsync(album, cancellationToken);
            await repo.SaveChangesAsync(cancellationToken);

            return new CreateAlbumResult(Guid.NewGuid());
        }
    }
}
