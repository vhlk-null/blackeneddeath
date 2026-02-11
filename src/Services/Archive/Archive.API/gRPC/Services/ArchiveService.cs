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

            if (album == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            var albumModel = album.Adapt<GetAlbumResponse>();
            return albumModel;
        }

        public override async Task<GetBandResponse> GetBandById(GetBandRequest request, ServerCallContext context)
        {
            var band = await repo.GetByAsync<Band>(a => a.Id == Guid.Parse(request.Id));

            if (band == null)
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid request object."));

            var bandModel = band.Adapt<GetBandResponse>();

            return bandModel;
        }
    }
}
