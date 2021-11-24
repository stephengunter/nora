using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using NLog.Web;
using Microsoft.AspNetCore;
using ApplicationCore.DataAccess;
using System.Threading.Tasks;
using ApplicationCore.Helpers;

namespace Web
{
    public class Program
    {
		public static void Main(string[] args)
		{
            var logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            try
            {
                var host = CreateWebHostBuilder(args).Build();
                logger.Debug("init main. environment: " + environment);

                if (environment.EqualTo("Development"))
                {
                    Task.Run(() => AppDBSeed.EnsureSeedData(host.Services).Wait());
                }

                host.Run();
            }
            catch (Exception exception)
            {
                //NLog: catch setup errors
                logger.Error(exception, "Stopped program because of exception");
                throw;
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                NLog.LogManager.Shutdown();
            }
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
           WebHost.CreateDefaultBuilder(args)
               .UseStartup<Startup>()
               .ConfigureLogging(logging =>
               {
                   logging.ClearProviders();
                   logging.SetMinimumLevel(LogLevel.Trace);
               })
               .UseNLog();  // NLog: Setup NLog for Dependency injection
    }
}
