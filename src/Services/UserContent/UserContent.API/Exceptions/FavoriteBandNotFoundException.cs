using BuildingBlocks.Exceptions;

namespace UserContent.API.Exceptions
{
    public class FavoriteBandNotFoundException : NotFoundException
    {
        public FavoriteBandNotFoundException(Guid Id) : base("FavoriteBand", Id)
        {
        }
    }
}
