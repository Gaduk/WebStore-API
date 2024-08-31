using Persistence.Context;
using Web_API.Extensions;

namespace Web_API;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().MigrateDatabase<ApplicationDbContext>().Run();
    }

    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}