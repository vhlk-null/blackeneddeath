namespace UserContent.API.UserContent.UserProfile.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResult>;

    public record GetUserProfileResult(UserProfileInfo UserProfileInfo);

    public class GetUserProfileQueryValidator : AbstractValidator<GetUserProfileQuery>
    {
        public GetUserProfileQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        }
    }

    public class GetUserProfileHandler(IRepository<UserContentContext> repo) : IQueryHandler<GetUserProfileQuery, GetUserProfileResult>
    {
        public async ValueTask<GetUserProfileResult> Handle(GetUserProfileQuery query, CancellationToken cancellationToken)
        {
            var userProfile = await repo.GetWithIncludesAsync<UserProfileInfo>(
                a => a.UserId == query.UserId,
                cancellationToken,
                [
                    a => a.FavoriteAlbums,
                    a => a.FavoriteBands
                ]);

            return new GetUserProfileResult(userProfile);
        }
    }
}
