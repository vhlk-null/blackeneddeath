namespace Library.Application.Dtos;

public record GenreCardDetailDto(
    Guid Id,
    string Name,
    string Description,
    List<TagDto> Tags,
    List<GenreDto> Genres,
    string? CoverUrl);