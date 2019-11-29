using Agiper;
using Agiper.Object;
using ServiceStack;
using System;
using System.Threading.Tasks;

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
                instance.NotifyChannel(Host.Current.ChannelName, signal);
            }
            return Task.CompletedTask;
        }
    }
}