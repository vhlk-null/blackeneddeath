namespace UserContent.Application.Services.gRPC;

public class LibraryGrpcService(
    LibraryProtoService.LibraryProtoServiceClient client,
    ILogger<LibraryGrpcService> logger)
    : ILibraryService
{
    public async Task<Album?> GetAlbumByIdAsync(Guid albumId, CancellationToken ct = default)
    {
        try
        {
            GetAlbumResponse? response = await client.GetAlbumByIdAsync(
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

    public async Task<Band?> GetBandByIdAsync(Guid bandId, CancellationToken ct = default)
    {
        try
        {
            GetBandResponse? response = await client.GetBandByIdAsync(
                new GetBandRequest { Id = bandId.ToString() },
                cancellationToken: ct);

            return response.Adapt<Band>();
        }
        catch (Grpc.Core.RpcException ex) when (ex.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "gRPC call to Library.GetBandById failed for bandId {BandId}", bandId);
            throw;
        }
    }
}
