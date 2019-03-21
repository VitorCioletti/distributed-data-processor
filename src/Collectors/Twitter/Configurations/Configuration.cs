namespace Twitter
{
    using System;
    using System.IO;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;

    public class Configuration
    {
        public static CollectorConfiguration Collector { get; set; }

        public static IntegrationsConfiguration Integrations { get; set; }

        private static IConfigurationRoot _configuration;

        public static void Initialize()
        {
            var directory = $"{Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)}";
            var builder = new ConfigurationBuilder().SetBasePath(AppContext.BaseDirectory);

            builder.AddJsonFile($"{directory}/config.json");
            _configuration = builder.Build();

            _configuration.Get<Configuration>();
        }
    }
}