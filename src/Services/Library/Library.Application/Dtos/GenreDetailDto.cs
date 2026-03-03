namespace Library.Application.Dtos;

public record GenreDetailDto(Guid Id, string Name, Guid? ParentGenreId);
