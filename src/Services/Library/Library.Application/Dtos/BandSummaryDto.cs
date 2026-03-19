namespace Library.Application.Dtos;

public record BandSummaryDto(
    Guid? Id,
    string Name,
    string? Slug);
