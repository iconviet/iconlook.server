using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using StackExchange.Redis;
using Syncfusion.EJ2.Blazor;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;

namespace Iconlook.Service.Web
{
    public class Program : ProgramBase
    {
        public static Task Main()
        {
            ConfigureServices = host => services =>
            {
                services.AddServerSideBlazor();
                services.AddSyncfusionBlazor();
                services.Configure<KestrelServerOptions>(x =>
                {
                    x.AllowSynchronousIO = true;
                });
                services.Configure<HubOptions>(x =>
                {
                    x.EnableDetailedErrors = true;
                    x.MaximumReceiveMessageSize = 1024 * 1024;
                });
                services.Configure<ForwardedHeadersOptions>(x =>
                {
                    x.KnownProxies.Clear();
                    x.KnownNetworks.Clear();
                    x.ForwardedHeaders = ForwardedHeaders.All;
                });
                services.AddWebMarkupMin(x =>
                {
                    x.DisablePoweredByHttpHeaders = true;
                    x.AllowMinificationInDevelopmentEnvironment = true;
                }).AddHtmlMinification(x => x.MinificationSettings.RemoveHtmlComments = false);
                var connection = $"{host.HostConfiguration.GetConnectionString("redis")},password={host.DefaultPassword}";
                if (connection.HasValue())
                {
                    if (host.Environment == Environment.Localhost)
                    {
                        connection = connection.Replace("redis", "localhost");
                    }
                    
                    services.AddSignalR().AddMessagePackProtocol().AddStackExchangeRedis(connection);
                }
                if (!OperatingSystem.IsWindows)
                {
                    services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                        .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
                }
                else
                {
                    services.AddDataProtection()
                        .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name)
                        .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(connection), host.ProjectName);
                }
                services.AddRazorPages(x => x.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute()));
            };
            ConfigureApplication = host => application =>
            {
                application.Use((context, next) =>
                {
                    if (host.Environment != Environment.Localhost)
                    {
                        context.Request.Scheme = "https";
                    }
                    context.Response.OnStarting(state =>
                    {
                        var http_context = (HttpContext) state;
                        var hostname = System.Environment.GetEnvironmentVariable("HOSTNAME");
                        http_context.Response.Headers["X-Powered-By"] =
                            (hostname.HasValue() ? $"{hostname}.{host.EndpointName}" : host.EndpointInstanceId).ToLower();
                        return Task.CompletedTask;
                    }, context);
                    return next();
                });
                application.UseForwardedHeaders();
                application.UseStaticFiles();
                application.UseWhen(context => context.Request.Path.StartsWithSegments("/api") ||
                                               context.Request.Path.StartsWithSegments("/sse"), builder => builder.UseServiceStack(host));
                application.UseRouting();
                application.UseWebMarkupMin();
                application.UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_Host");
                });
                ConfigureApplicationDefault(host, application); // TODO: Remove this
            };
            SyncfusionLicenseProvider.RegisterLicense("MTgzMzE3QDMxMzcyZTM0MmUzMGsxUUk0Rkp6Vk9zaGNsaWlIQVBRWlhhNEpYa0hNRlFOWUVYUFBtcFFIWDg9");
            return StartAsync(new WebHost(), 80);
        }
    }
}