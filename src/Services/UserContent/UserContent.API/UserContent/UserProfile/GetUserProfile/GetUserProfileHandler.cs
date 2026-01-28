namespace UserContent.API.UserContent.UserProfile.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResult>;

    public record GetUserProfileResult(UserProfileInfo UserProfileInfo);
    public class GetUserProfileHandler : IQueryHandler<GetUserProfileQuery, GetUserProfileResult>
    {
        public async Task<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            // TODO: get user from database
            // var userProfile = await repo.GetBy(query.UserId);

            return new GetUserProfileResult(new UserProfileInfo());
        }
    }
}
