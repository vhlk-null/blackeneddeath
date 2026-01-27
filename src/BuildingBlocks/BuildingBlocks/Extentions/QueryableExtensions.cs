using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;

namespace BuildingBlocks.Extentions
{
    public record PagedQuery<TResponse> : IQuery<PagedResult<TResponse>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public string? SortBy { get; init; }
        public bool SortDescending { get; init; } = true;
    }

    public record PagedResult<T>
    {
        public List<T> Items { get; init; } = new();
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalCount { get; init; }
        public int TotalPages { get; init; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedResultAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize, CancellationToken cancellationToken = default)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            return new PagedResult<T>
            {
                Items = items,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount,
                TotalPages = totalPages
            };
        }
    }
}
