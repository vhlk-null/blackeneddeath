namespace Library.Application.Services.Genres.Queries.GetGenresByParent;

public record GetGenresByParentQuery(Guid ParentId) : BuildingBlocks.CQRS.IQuery<GetGenresByParentResult>;

public record GetGenresByParentResult(IEnumerable<GenreDetailDto> Genres);
