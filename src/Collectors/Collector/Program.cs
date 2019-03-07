namespace Collector
{
    using System;
    using System.Threading;

    class Program
    {
        private static Collector _collector;

        private static MessageQueue _messageQueue;

        static void Main(string[] args)
        {
            Initialize();

            Thread.Sleep(-1);
        }

        private static void Initialize()
        {
            Log.Initialize();
            Configuration.Initialize();

            _collector = new Collector();
            _messageQueue = new MessageQueue();

            _collector.Send += (message) => _messageQueue.Send(message);

            _collector.Collect();

            Log.Write("Initialized", "Initialized program.");
        }

        private static void ConfigureCurrentDomainEvents()
        {
            var currentDomain = AppDomain.CurrentDomain;
            
            Action finalizeConnections = () =>
            {
                _collector.Finalize();
                _messageQueue.Finalize();
            };

            currentDomain.UnhandledException += (_, e) => 
            {
                var exception = (Exception)e.ExceptionObject;

                finalizeConnections();

                Log.Write("Error", $"Unhandled exception: {exception.Message} {exception.StackTrace}");
            };

            currentDomain.ProcessExit += (_, e) => 
            {
                finalizeConnections();

                Log.Write("Finalized", "Flagged to end.");
            };
        }
    }
}