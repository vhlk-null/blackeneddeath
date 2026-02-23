namespace Library.Domain.Models;

public class StreamingLink : Entity<StreamingLinkId>
{
    public StreamingPlatform Platform { get; private set; }
    public string EmbedCode { get; private set; } = null!;

    internal StreamingLink(StreamingPlatform platform, string embedCode)
    {
        Platform = platform;
        EmbedCode = embedCode;
    }
}
