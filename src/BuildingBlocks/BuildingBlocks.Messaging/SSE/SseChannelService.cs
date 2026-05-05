using System.Collections.Concurrent;
using System.Threading.Channels;

namespace BuildingBlocks.Messaging.SSE
{
    public class SseChannelService
    {
        private readonly ConcurrentDictionary<string, Channel<string>> _channel = new ConcurrentDictionary<string, Channel<string>>();

        public ChannelReader<string> Subscribe(string userId)
        {
            return _channel.GetOrAdd(userId, _ => Channel.CreateUnbounded<string>()).Reader;
        }

        public void Unsubscribe(string userId)
        {
            if (_channel.TryRemove(userId, out var channel))
                channel.Writer.TryComplete();
        }

        public async Task PublishAsync(string userId, string message)
        {
            if (_channel.TryGetValue(userId, out var channel))
                await channel.Writer.WriteAsync(message);
        }
    }
}
