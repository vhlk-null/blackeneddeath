namespace Library.Application.Dtos;

public record GenreCardDto(
    Guid Id,
    string Name,
    string Description,
    string? CoverUrl);
