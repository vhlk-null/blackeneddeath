using UserContent.Application.UserContent.UserProfile.GetUserProfile;

namespace UserContent.API.Endpoints.UserProfile;

public record GetUserProfileResponse(UserProfileDto UserProfile);

public class GetUserProfileEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/profile/{userId:guid}",
                async (Guid userId, ISender sender) =>
                {
                    var result = await sender.Send(new GetUserProfileQuery(userId));

                    return Results.Ok(result);
                })
            .WithName("GetUserProfile")
            .Produces<GetUserProfileResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get UserProfile")
            .WithDescription("Get UserProfile");
    }
}
