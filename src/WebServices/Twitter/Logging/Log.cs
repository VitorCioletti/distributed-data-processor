namespace Twitter.Logging
{
    using System;
    using LLibrary;

    public static class Log
    {
        private static L _log;

        public static void Initialize()
        {
            _log = new L(
                deleteOldFiles: new TimeSpan(10, 0, 0),
                directory: "logs"
            );

            AppDomain.CurrentDomain.UnhandledException += (_, e) => Write("Error", e.ExceptionObject);

            Write("Log", "Initialized.");
        }

        public static void Write(string label, object informacao)
        {
            Console.WriteLine($"{DateTime.Now} - {label.ToUpper()} - {informacao}");
            _log.Log(label.ToUpper(), informacao);
        }
    }
}