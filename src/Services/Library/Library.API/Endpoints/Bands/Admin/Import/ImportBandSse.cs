using System.Text.Json;
using System.Threading.Channels;
using Library.Application.Services.Import;
using Library.Application.Services.Import.Commands.ImportBand;

namespace Library.API.Endpoints.Bands.Admin.Import;

public class ImportBandSse : ICarterModule
{
    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new JsonStringEnumConverter() }
    };

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/admin/import/band/stream",
                async (string mbId, string bandName, ISender sender, HttpResponse response, CancellationToken ct) =>
                {
                    response.Headers.ContentType = "text/event-stream";
                    response.Headers.CacheControl = "no-cache";
                    response.Headers.Connection = "keep-alive";

                    var channel = Channel.CreateUnbounded<ImportProgressEvent>(
                        new UnboundedChannelOptions { SingleReader = true, SingleWriter = false });

                    var progress = new Progress<ImportProgressEvent>(evt =>
                        channel.Writer.TryWrite(evt));

                    // Run import in background — not tied to HTTP cancellation
                    var importTask = Task.Run(async () =>
                    {
                        try
                        {
                            await sender.Send(new ImportBandCommand(mbId, bandName, progress), CancellationToken.None);
                        }
                        finally
                        {
                            channel.Writer.Complete();
                        }
                    });

                    // Stream events to client until channel is done or client disconnects
                    await foreach (ImportProgressEvent evt in channel.Reader.ReadAllAsync(ct))
                    {
                        try
                        {
                            string json = JsonSerializer.Serialize(evt, JsonOpts);
                            await response.WriteAsync($"data: {json}\n\n", ct);
                            await response.Body.FlushAsync(ct);
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                    }

                    // Wait for import to finish even if client disconnected
                    await importTask;
                })
            .WithName("ImportBandStream")
            .WithTags("Admin")
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status403Forbidden)
            .RequireAuthorization("AdminOnly")
            .WithSummary("Import band from MusicBrainz with SSE progress");
    }
}
