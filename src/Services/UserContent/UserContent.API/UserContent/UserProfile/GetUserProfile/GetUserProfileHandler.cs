namespace UserContent.API.UserContent.UserProfile.GetUserProfile
{
    public record GetUserProfileQuery(Guid UserId) : IQuery<GetUserProfileResult>;

    public record GetUserProfileResult(UserProfileDto UserProfile);

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

            var userProfile = await repo.GetWithIncludesAsync<UserProfileInfo>(a => a.UserId == query.UserId,
                q => q
                    .Include(a => a.FavoriteAlbums).ThenInclude(a => a.Album)
                    .Include(a => a.FavoriteBands).ThenInclude(a => a.Band),
                cancellationToken);

            return userProfile == null ? throw new UserProfileNotFoundException(query.UserId) : new GetUserProfileResult(userProfile.Adapt<UserProfileDto>());
        }
    }
}
