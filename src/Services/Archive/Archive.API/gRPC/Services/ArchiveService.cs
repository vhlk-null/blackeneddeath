using Archive.Grpc;
using Grpc.Core;

namespace Archive.API.gRPC.Services
{
    public class ArchiveService : ArchiveProtoService.ArchiveProtoServiceBase
    {
        private readonly ILogger<ArchiveService> _logger;

        public ArchiveService(ILogger<ArchiveService> logger)
        {
            _logger = logger;
        }

        public override Task<CheckAlbumExistsResponse> CheckAlbumExists(CheckAlbumExistsRequest request, ServerCallContext context)
        {
            return base.CheckAlbumExists(request, context);
        }
    }
}
