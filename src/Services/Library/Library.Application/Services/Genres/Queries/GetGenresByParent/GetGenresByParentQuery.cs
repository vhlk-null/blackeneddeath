namespace Library.Application.Genres.Queries.GetGenresByParent;

public record GetGenresByParentQuery(Guid ParentId) : BuildingBlocks.CQRS.IQuery<GetGenresByParentResult>;

public record GetGenresByParentResult(IEnumerable<GenreDetailDto> Genres);
