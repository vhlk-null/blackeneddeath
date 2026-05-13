namespace Library.Application.Abstractions;

public interface IBandDetailCache
{
    Task<BandDetailDto?> GetAsync(string slug, CancellationToken cancellationToken = default);
    Task SetAsync(string slug, Guid bandId, BandDetailDto dto, CancellationToken cancellationToken = default);
    Task InvalidateAsync(Guid bandId, CancellationToken cancellationToken = default);
}
