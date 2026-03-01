namespace UserContent.Application.UserContent.UserProfile.GetUserProfile;

public class GetUserProfileHandler(IUserContentRepository repo)
    : IQueryHandler<GetUserProfileQuery, GetUserProfileResult>
{
    public async ValueTask<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
    {
        var userProfile = await repo.GetUserProfileWithDetailsAsync(query.UserId, cancellationToken);

        return userProfile is null
            ? throw new UserProfileNotFoundException(query.UserId)
            : new GetUserProfileResult(userProfile.Adapt<UserProfileDto>());
    }
}
