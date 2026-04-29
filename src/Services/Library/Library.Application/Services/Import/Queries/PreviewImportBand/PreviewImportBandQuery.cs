namespace Library.Application.Services.Import.Queries.PreviewImportBand;

public record PreviewImportBandQuery(string MbId) : BuildingBlocks.CQRS.IQuery<BandPreviewResult>;
