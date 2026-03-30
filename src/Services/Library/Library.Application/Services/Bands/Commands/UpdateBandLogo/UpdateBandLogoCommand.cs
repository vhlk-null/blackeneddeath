namespace Library.Application.Services.Bands.Commands.UpdateBandLogo;

public record UpdateBandLogoCommand(
    Guid BandId,
    Stream Logo,
    string LogoContentType,
    string LogoFileName)
    : BuildingBlocks.CQRS.ICommand<UpdateBandLogoResult>;

public record UpdateBandLogoResult(bool IsSuccess);
