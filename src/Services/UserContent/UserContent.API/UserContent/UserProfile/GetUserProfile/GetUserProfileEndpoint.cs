namespace UserContent.API.UserContent.UserProfile.GetUserProfile
{
    //public record GetUserProfileRequest(Guid UserId);

    public record GetUserProfileResponse(UserProfileInfo UserProfileInfo);
    public class GetUserProfileEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/profile/{userId:guid}",
                async (Guid userId, ISender sender) =>
                {
                    var result = await sender.Send(new GetUserProfileQuery(userId));

                    var response = result.Adapt<GetUserProfileResponse>();

                    return Results.Created($"/profile/{response.UserProfileInfo.UserId}", response);
                })
            .WithName("GetUserProfile")
            .Produces<GetUserProfileResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .WithSummary("Get UserProfile")
            .WithDescription("Get UserProfile");
        }
    }
}
