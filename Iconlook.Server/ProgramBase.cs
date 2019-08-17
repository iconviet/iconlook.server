using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Agiper;
using Agiper.Server;
using FluentMigrator.Runner;
using Hangfire;
using Hangfire.MemoryStorage;
using Hangfire.Storage;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceStack;
using ServiceStack.Redis;
using Environment = Agiper.Environment;

namespace Iconlook.Server
{
    public abstract class ProgramBase
    {
        public static Action<HostBase, ILoggingBuilder> ConfigureLogging = delegate { };
        public static Action<HostBase, IServiceCollection> ConfigureServices = delegate { };
        public static Action<HostBase, IApplicationBuilder> ConfigureApplication = delegate { };

        public static Task StartAsync<T>(int port, Action<HostBase> configure = null) where T : HostBase, new()
        {
            var host = new T();
            configure?.Invoke(host);
            return StartAsync(host, port);
        }

        public static async Task StartAsync(HostBase host, int port)
        {
            var builder = new WebHostBuilder();
            builder.UseSerilog()
                .UseKestrel(x => x.ListenAnyIP(port))
                .UseEnvironment(host.Environment.ToString())
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureLogging(logging => ConfigureLogging(host, logging))
                .ConfigureServices(services =>
                {
                    ConfigureServices(host, services);
                    services.AddHangfire(configuration => { });
                    services.AddFluentMigratorCore()
                        .AddLogging(l => l.AddFluentMigratorConsole())
                        .ConfigureRunner(r => r.AddSqlServer2016().ScanIn(Assembly.GetEntryAssembly()).For.Migrations())
                        .BuildServiceProvider(false);
                })
                .Configure(application =>
                {
                    ConfigureApplication(host, application);
                    if (ServiceStackHost.Instance == null) application.UseServiceStack(host);
                    GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 0 });
                    switch (host.HangfireJobPersistence)
                    {
                        default: throw new ArgumentOutOfRangeException();
                        case HangfireJobPersistence.Memory:
                            GlobalConfiguration.Configuration.UseMemoryStorage();
                            break;
                        case HangfireJobPersistence.Redis:
                            GlobalConfiguration.Configuration.UseRedisStorage(host.Resolve<IRedisClient>().Host);
                            break;
                        case HangfireJobPersistence.SqlServer:
                            GlobalConfiguration.Configuration.UseSqlServerStorage(host.Resolve<IDbConnection>().ConnectionString);
                            JobStorage.Current.GetConnection().GetRecurringJobs().ForEach(job => RecurringJob.RemoveIfExists(job.Id));
                            break;
                    }
                    application.UseHangfireDashboard("/hangfire", new DashboardOptions { Authorization = new[] { new HangfireAuthorizationFilter() } });
                    application.UseHangfireServer(new BackgroundJobServerOptions { ServerName = host.EndpointName, Activator = new AutofacJobActivator(host.Container) });
                });
            using (var wait = builder.Build())
            {
                await wait.StartAsync();
                Console.Title = host.ServiceName.ToUpper();
                Log.Information("■ port        : {Port}", port);
                Log.Information("■ name        : {Name}", host.ServiceName.ToUpper());
                if (host.Environment == Environment.Localhost)
                {
                    Log.Information("■ version     : {Version}", Assembly.GetEntryAssembly().InformationVersion() + $"-{host.Environment.ToString().ToLower()}");
                    Log.Information("■ started     : {Started:dd MMMM yyyy hh:mm:ss tt}", DateTime.Now);
                }
                else
                {
                    Log.Information("■ started     : {Started:dd MMMM yyyy hh:mm:ss tt}", DateTime.Now);
                    Log.Information("■ version     : {Version}", Assembly.GetEntryAssembly().InformationVersion() + $"-{host.Environment.ToString().ToLower()}");
                }
                Log.Information("**************************************************");
                if (host.Exceptions.Any())
                {
                    Log.Fatal("{Count} configuration error(s)", host.Exceptions.Count);
                    host.Exceptions.Each(x => Log.Fatal($"error #{{Index}}: {x.Key.ToLower()} -> {char.ToLower(x.Value.Message[0]) + x.Value.Message.Substring(1)} ({x.Value.InnerException?.Message.ToLower()}) [{x.Value.Source.ToLower()}]", host.Exceptions.ToList().IndexOf(x) + 1));
                    Log.Fatal("------------------------------------------");
                    Log.Fatal("HOST CONFIGURATION FAILURE. START ABORTED.");
                    Log.Fatal("------------------------------------------");
                    Console.ReadLine();
                }
                else
                {
                    await host.StartAsync();
                    await wait.WaitForShutdownAsync();
                }
            }
        }
    }
}