namespace Library.API.gRPC.Services;

public class LibraryService(IRepository<LibraryContext> repo) : LibraryProtoService.LibraryProtoServiceBase
{
    public override async Task<GetAlbumResponse> GetAlbumById(GetAlbumRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out Guid guid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        AlbumId id = AlbumId.Of(guid);
        Album? album = await repo.GetByAsync<Album>(a => a.Id == id);

        return album == null
            ? throw new RpcException(new Status(StatusCode.NotFound, $"Album with id '{request.Id}' was not found."))
            : album.Adapt<GetAlbumResponse>();
    }

    public override async Task<GetBandResponse> GetBandById(GetBandRequest request, ServerCallContext context)
    {
        if (!Guid.TryParse(request.Id, out Guid guid))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

        BandId bandId = BandId.Of(guid);
        Band? band = await repo.GetByAsync<Band>(a => a.Id == bandId);

        return band == null
            ? throw new RpcException(new Status(StatusCode.NotFound, $"Band with id '{request.Id}' was not found."))
            : band.Adapt<GetBandResponse>();
    }
}