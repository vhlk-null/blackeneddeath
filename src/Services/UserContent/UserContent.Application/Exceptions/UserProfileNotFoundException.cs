namespace UserContent.Application.Exceptions;

public class UserProfileNotFoundException : NotFoundException
{
    public UserProfileNotFoundException(Guid id) : base("UserProfile", id)
    {
    }
}
