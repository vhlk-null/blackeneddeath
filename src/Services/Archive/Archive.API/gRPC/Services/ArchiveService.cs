using Archive.API.Models;
using Archive.Grpc;
using Grpc.Core;

namespace Archive.API.gRPC.Services
{
    public class ArchiveService(IRepository<ArchiveContext> repo) : ArchiveProtoService.ArchiveProtoServiceBase
    {
        public override async Task<GetAlbumResponse> GetAlbumById(GetAlbumRequest request, ServerCallContext context)
        {
            var album = await repo.GetByAsync<Album>(a => a.Id == Guid.Parse(request.Id));

            return album == null
                ? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."))
                : album.Adapt<GetAlbumResponse>();
        }

        public override async Task<GetBandResponse> GetBandById(GetBandRequest request, ServerCallContext context)
        {
            var band = await repo.GetByAsync<Band>(a => a.Id == Guid.Parse(request.Id));

            return band == null
                ? throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."))
                : band.Adapt<GetBandResponse>();
        }
    }
}
