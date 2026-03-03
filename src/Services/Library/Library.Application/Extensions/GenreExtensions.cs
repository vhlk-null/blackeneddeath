namespace Library.Application.Extensions;

public static class GenreExtensions
{
    public static GenreDetailDto ToGenreDetailDto(this Genre genre) => new(
        genre.Id.Value,
        genre.Name,
        genre.ParentGenreId?.Value);
}
