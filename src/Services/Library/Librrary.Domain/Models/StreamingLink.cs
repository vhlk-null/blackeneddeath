namespace Library.Domain.Models;

public class StreamingLink : Entity<StreamingLinkId>
{
    public StreamingPlatform Platform { get; private set; }
    public string EmbedCode { get; private set; } = null!;

    private StreamingLink() { }

    internal StreamingLink(StreamingPlatform platform, string embedCode)
    {
        Id = StreamingLinkId.Of(Guid.NewGuid());
        Platform = platform;
        EmbedCode = embedCode;
    }
}
