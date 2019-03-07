namespace Collector
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;

    public class Configuration
    {
        public static string Host { get; set; }

        public static int Port { get; set; }

        public static string VirtualHost { get; set; }

        public static string User { get; set; }

        public static string Password { get; set; }

        public static string Exchange { get; set; }

        public static string RoutingKey { get; set; }

        private static IConfigurationRoot _configuration;

        public static void Initialize()
        {
            var diretorio = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);

            builder.AddJsonFile($"{diretorio}/Configuration/configuration.json");
            _configuration = builder.Build();
        }
    }
}