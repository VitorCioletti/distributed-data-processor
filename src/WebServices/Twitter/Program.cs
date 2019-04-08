namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Logging;

    public class Program
    {
        public static void Main(string[] args)
        {
            Log.Initialize();

            CreateWebHostBuilder(args).Build().RunAsync().ContinueWith(t =>
            {
                Log.Write("Web Service", $"{t.Exception.Message} {t.Exception.StackTrace}");
            }, TaskContinuationOptions.OnlyOnFaulted);
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
