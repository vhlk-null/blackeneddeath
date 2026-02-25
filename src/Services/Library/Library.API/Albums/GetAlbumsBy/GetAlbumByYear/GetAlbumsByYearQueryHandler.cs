using Library.API.Data;
using Library.Domain.Models;
using Library.Infrastructure.Data;

namespace Library.API.Albums.GetAlbumsBy.GetAlbumByYear;

public record GetAlbumsByYearQuery(int ReleaseDate) : IQuery<GetAlbumsByYearResult>;
public record GetAlbumsByYearResult(IEnumerable<Album> Albums);

internal class GetAlbumByByYearQueryHandler(IRepository<LibraryContext> repo)
    : IQueryHandler<GetAlbumsByYearQuery, GetAlbumsByYearResult>
{
    public async ValueTask<GetAlbumsByYearResult> Handle(GetAlbumsByYearQuery query, CancellationToken cancellationToken)
    {
        var albums = await repo.FilterAsync<Album>(a => a.AlbumRelease.ReleaseYear == query.ReleaseDate);

        return new GetAlbumsByYearResult(albums);
    }
}