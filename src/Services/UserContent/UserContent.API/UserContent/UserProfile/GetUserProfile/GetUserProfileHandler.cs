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
            // TODO: get user from database

            var userProfile = await repo.Filter<UserProfileInfo>(a => a.UserId == query.UserId)
                .Include(a => a.FavoriteAlbums)
                .Include(a => a.FavoriteBands)
                .FirstAsync();

            return new GetUserProfileResult(userProfile);
        }
    }
}
