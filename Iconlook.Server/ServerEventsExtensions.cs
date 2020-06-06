using System;
using System.Threading.Tasks;
using Iconviet;
using Iconviet.Object;
using Iconviet.Server;
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
                instance.NotifyChannel(HostBase.Configuration.ChannelId, signal);
            }
            return Task.CompletedTask;
        }
    }
}