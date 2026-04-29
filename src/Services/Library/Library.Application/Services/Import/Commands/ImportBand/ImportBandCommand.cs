namespace Library.Application.Services.Import.Commands.ImportBand;

public record ImportBandCommand(string MbId, string BandName, IProgress<ImportProgressEvent>? Progress = null, IReadOnlySet<string>? SelectedAlbumMbIds = null) : BuildingBlocks.CQRS.ICommand<ImportBandResult>;

public record ImportBandResult(
    Guid BandId,
    string BandName,
    int AlbumsImported,
    int AlbumsSkipped,
    List<string> CreatedLabels);
