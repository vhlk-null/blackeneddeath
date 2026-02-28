namespace Library.Application.Albums.Queries.GetAlbums;

public record GetAlbumsQuery(Guid Id) : BuildingBlocks.CQRS.IQuery<AlbumDto>;

public class GetAlbumsQueryHandler() 
{
    
}