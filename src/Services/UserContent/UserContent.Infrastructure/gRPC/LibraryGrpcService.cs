namespace UserContent.Infrastructure.gRPC;

public class LibraryGrpcService(
    LibraryProtoService.LibraryProtoServiceClient client,
    ILogger<LibraryGrpcService> logger)
    : ILibraryService
{
    public async Task<Album?> GetAlbumByIdAsync(Guid albumId, CancellationToken ct = default)
    {
        try
        {
            var response = await client.GetAlbumByIdAsync(
                new GetAlbumRequest { Id = albumId.ToString() },
                cancellationToken: ct);

            return response.Adapt<Album>();
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "gRPC call to Library.GetAlbumById failed for albumId {AlbumId}", albumId);
            throw;
        }
    }
}
