namespace Library.Application.Dtos;

public record GenreDto(
    Guid Id,
    string Name,
    string? Slug,
    bool IsPrimary);
