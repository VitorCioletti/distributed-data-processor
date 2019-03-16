namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class Program
    {
        private static Collector _collector;

        private static MessageQueue _messageQueue;

        static void Main(string[] args)
        {
            try
            {
                Initialize();
            }
            catch (Exception e)
            {
                Log.Write("Initialization", $"An error has occurred. {e.Message} {e.StackTrace}");

                Environment.Exit(0);   
            }

            Thread.Sleep(-1);
        }

        private static void Initialize()
        {
            Configuration.Initialize();
            Backup.Initialize();
            Twitter.Initialize();

            Thread.Sleep(-1);

            _collector = new Collector();
            _messageQueue = new MessageQueue();

            Func<IEnumerable<object>, bool> trySend = message => _messageQueue.TrySend(message);
 
            _collector.TrySend += trySend;
            Backup.TrySend += trySend;

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