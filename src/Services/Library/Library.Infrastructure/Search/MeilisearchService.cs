namespace Library.Infrastructure.Search;

public class MeilisearchService(MeilisearchClient client) : ISearchService
{
    public async Task IndexAlbumAsync(AlbumSearchDocument document, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Albums);
        await index.AddDocumentsAsync([document], cancellationToken: ct);
    }

    public async Task IndexBandAsync(BandSearchDocument document, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Bands);
        await index.AddDocumentsAsync([document], cancellationToken: ct);
    }

    public async Task RemoveAlbumAsync(string id, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Albums);
        await index.DeleteOneDocumentAsync(id, cancellationToken: ct);
    }

    public async Task RemoveBandAsync(string id, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Bands);
        await index.DeleteOneDocumentAsync(id, cancellationToken: ct);
    }

    public async Task<IEnumerable<AlbumSearchDocument>> SearchAlbumsAsync(string query, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Albums);
        var result = await index.SearchAsync<AlbumSearchDocument>(query, cancellationToken: ct);
        return result.Hits;
    }

    public async Task<IEnumerable<BandSearchDocument>> SearchBandsAsync(string query, CancellationToken ct = default)
    {
        var index = client.Index(SearchIndexes.Bands);
        var result = await index.SearchAsync<BandSearchDocument>(query, cancellationToken: ct);
        return result.Hits;
    }
}
