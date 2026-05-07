using BuildingBlocks.Exceptions;

namespace Activity.Application.Exceptions;

public class ActivityNotFoundException(Guid userId)
    : NotFoundException($"No activity found for user '{userId}'.");
