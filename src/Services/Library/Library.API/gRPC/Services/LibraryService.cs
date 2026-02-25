using Grpc.Core;
using Library.API.Data;
using Library.Domain.Models;
using Library.Grpc;
using Library.Infrastructure.Data;

namespace Library.API.gRPC.Services;

public class LibraryService(IRepository<LibraryContext> repo) : LibraryProtoService.LibraryProtoServiceBase
{
    public override async Task<GetAlbumResponse> GetAlbumById(GetAlbumRequest request, ServerCallContext context)
    {
        var album = await repo.GetByAsync<Album>(a => a.Id.Value == Guid.Parse(request.Id));

        return album == null
            ? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."))
            : album.Adapt<GetAlbumResponse>();
    }

    public override async Task<GetBandResponse> GetBandById(GetBandRequest request, ServerCallContext context)
    {
        var band = await repo.GetByAsync<Band>(a => a.Id.Value == Guid.Parse(request.Id));

        return band == null
            ? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."))
            : band.Adapt<GetBandResponse>();
    }
}