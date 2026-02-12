namespace UserContent.API.Exceptions
{
    public class UserProfileNotFoundException : NotFoundException
    {
        public UserProfileNotFoundException(Guid Id) : base("UserProfile", Id)
        {
        }
    }
}
