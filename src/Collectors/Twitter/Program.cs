namespace Twitter
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Initialize();

                Thread.Sleep(-1);
            }
            catch (Exception e)
            {
                Log.Write("Initialization", $"An error has occurred. {e.Message} {e.StackTrace}");

            }
        }

        private static void Initialize()
        {
            ConfigureCurrentDomainEvents();

            Configuration.Initialize();
            Backup.Initialize();
            MessageQueue.Initialize();
            Twitter.Initialize();

            Func<Message, bool> trySend = message => MessageQueue.TrySend(message);
 
            Collector.TrySend += trySend;
            Backup.TrySend += trySend;
            
            Twitter.StartTweetStreaming();

            Log.WriteInitialized(typeof(Program));
        }

        private static void ConfigureCurrentDomainEvents()
        {
            var currentDomain = AppDomain.CurrentDomain;
            
            Action finalizeConnections = () =>
            {
                MessageQueue.Finalize();
                Backup.Finalize();
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
