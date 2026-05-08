using System.Collections.Concurrent;
using System.Threading.Channels;

namespace BuildingBlocks.Messaging.SSE
{
    public class SseChannelService
    {
        private readonly ConcurrentDictionary<string, List<Channel<string>>> _channels = new();

        public ChannelReader<string> Subscribe(string userId)
        {
            Channel<string> channel = Channel.CreateBounded<string>(new BoundedChannelOptions(100)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleReader = true,
                SingleWriter = false
            });

            _channels.AddOrUpdate(userId,
                _ => [channel],
                (_, list) => { lock (list) { list.Add(channel); } return list; });

            return channel.Reader;
        }

        public void Unsubscribe(string userId, ChannelReader<string> reader)
        {
            if (!_channels.TryGetValue(userId, out List<Channel<string>>? list)) return;

            lock (list)
            {
                Channel<string>? channel = list.FirstOrDefault(c => c.Reader == reader);
                if (channel is null) return;
                channel.Writer.TryComplete();
                list.Remove(channel);
                if (list.Count == 0)
                    _channels.TryRemove(userId, out _);
            }
        }

        public async Task PublishAsync(string userId, string message)
        {
            if (!_channels.TryGetValue(userId, out List<Channel<string>>? list)) return;

            List<Channel<string>> snapshot;
            lock (list) { snapshot = [.. list]; }

            foreach (Channel<string> channel in snapshot)
                await channel.Writer.WriteAsync(message);
        }
    }
}
