namespace UserContent.Domain.Models;

public class CollectionBand
{
    public Guid CollectionId { get; set; }
    public Guid BandId { get; set; }
    public DateTime AddedDate { get; set; }
    public Collection Collection { get; set; } = null!;
    public Band Band { get; set; } = null!;
}
