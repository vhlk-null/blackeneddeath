namespace Library.Application.Search;

public interface ISearchService
{
    Task IndexAlbumAsync(AlbumSearchDocument document, CancellationToken
        ct = default);
    Task IndexBandAsync(BandSearchDocument document, CancellationToken ct
        = default);
    Task RemoveAlbumAsync(string id, CancellationToken ct = default);
    Task RemoveBandAsync(string id, CancellationToken ct = default);
    Task<IEnumerable<AlbumSearchDocument>> SearchAlbumsAsync(string
        query, CancellationToken ct = default);
    Task<IEnumerable<BandSearchDocument>> SearchBandsAsync(string query,
        CancellationToken ct = default);
}