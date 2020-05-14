using Microsoft.Extensions.DependencyInjection;
using Syncfusion.Blazor;

namespace Iconlook.Service.Web
{
    public static class SyncfusionBlazorExtensions
    {
        public static IServiceCollection AddSyncfusionBlazor(this IServiceCollection instance)
        {
            return instance.AddSyncfusionBlazor(false);
        }
    }
}