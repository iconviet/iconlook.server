using System;
using System.Threading.Tasks;
using Agiper;
using Agiper.Object;
using Agiper.Server;
using ServiceStack;

namespace Iconlook.Server
{
    public static class ServerEventsExtensions
    {
        public static Task Publish(this IServerEvents instance, ISignal signal)
        {
            signal.Id = Generate.LongId();
            signal.Timestamp = DateTimeOffset.UtcNow;
            if (signal.IsValid())
            {
                instance.NotifyChannel(ServerBase.Config.ChannelName, signal);
            }
            return Task.CompletedTask;
        }
    }
}