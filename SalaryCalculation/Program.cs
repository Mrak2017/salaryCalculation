using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SalaryCalculation.Models;

namespace SalaryCalculation
{
    public class Program
    {
        /** Версия программы
         * отвечает, в том числе за ревизию данных*/
        private static readonly int PROGRAM_VERSION = 1;

        public static void Main(string[] args)
        {
            IWebHost host = CreateWebHostBuilder(args).Build();

            RunDbInitializer(host);

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void RunDbInitializer(IWebHost host)
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<SalaryCalculationDBContext>();
                    DbInitializer.Initialize(context, PROGRAM_VERSION);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while seeding the database.");
                }
            }
        }
    }
}
