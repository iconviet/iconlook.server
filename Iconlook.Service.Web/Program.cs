using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceStack;
using Syncfusion.Licensing;
using WebMarkupMin.AspNetCore3;

namespace Iconlook.Service.Web
{
    public class Program : ProgramBase
    {
        public static async Task Main()
        {
            ConfigureServices = host => services =>
            {
                services.AddRazorPages();
                services.AddServerSideBlazor();
                services.Configure<KestrelServerOptions>(options =>
                {
                    options.AllowSynchronousIO = true;
                });
                services.Configure<HubOptions>(options =>
                {
                    options.EnableDetailedErrors = true;
                    options.MaximumReceiveMessageSize = 1024 * 1024;
                });
                services.AddWebMarkupMin(options =>
                {
                    options.AllowMinificationInDevelopmentEnvironment = true;
                }).AddHtmlMinification(options =>
                {
                    options.MinificationSettings.RemoveHtmlComments = false;
                });
                services.Configure<ForwardedHeadersOptions>(options =>
                {
                    options.KnownProxies.Clear();
                    options.KnownNetworks.Clear();
                    options.ForwardedHeaders = ForwardedHeaders.XForwardedHost;
                });
                if (!OperatingSystem.IsWindows)
                {
                    services.AddDataProtection()
                        .PersistKeysToFileSystem(new DirectoryInfo("/var/lib/dotnet"))
                        .SetApplicationName(Assembly.GetEntryAssembly().GetName().Name);
                }
                var connection_string = host.HostConfiguration.GetConnectionString("redis");
                if (connection_string.HasValue())
                {
                    if (host.Environment == Environment.Localhost)
                    {
                        connection_string = connection_string.Replace("redis", "localhost");
                    }
                    services.AddSignalR().AddMessagePackProtocol().AddStackExchangeRedis(connection_string);
                }
            };
            ConfigureApplication = host => application =>
            {
                if (host.Environment != Environment.Localhost)
                {
                    application.Use((context, next) =>
                    {
                        context.Request.Scheme = "https";
                        return next();
                    });
                }
                application.UseForwardedHeaders();
                application.UseStaticFiles();
                application.Use((context, next) =>
                {
                    if (context.Request.Path.StartsWithSegments("/api"))
                    {
                        var query = QueryHelpers.ParseQuery(context.Request.QueryString.Value);
                        var builder = new QueryBuilder(query.SelectMany(x => x.Value, (x, y) =>
                            new KeyValuePair<string, string>(x.Key.Replace("$", string.Empty).Replace("top", "take"), y)));
                        context.Request.QueryString = builder.ToQueryString();
                    }
                    return next();
                });
                application.UseWhen(context => context.Request.Path.StartsWithSegments("/api"), a => a.UseServiceStack(host));
                application.UseRouting();
                application.UseWebMarkupMin();
                application.UseEndpoints(x =>
                {
                    x.MapBlazorHub();
                    x.MapFallbackToPage("/_Host");
                });
                ConfigureApplicationDefault(host, application); // TODO: Remove this
            };
            SyncfusionLicenseProvider.RegisterLicense("MTI1OTM0QDMxMzcyZTMyMmUzMG0yUm01UnZ6U3pQMjdLdEM1Q3RSSE1YdHl2R0RmQWh2N0JuZEZrd1BTc2s9");
            await StartAsync(new WebHost(), 80);
        }
    }
}