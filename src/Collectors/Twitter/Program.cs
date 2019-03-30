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
                Log.WriteException("Initialized", e);
            }
            finally
            {
                Finalize();
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

        private static void Finalize()
        {
            MessageQueue.Finalize();
            Backup.Finalize();

            Log.WriteFinalized(typeof(Program));
        }

        private static void ConfigureCurrentDomainEvents()
        {
            var currentDomain = AppDomain.CurrentDomain;
            
            currentDomain.UnhandledException += (_, e) => 
            {
                var exception = (Exception)e.ExceptionObject;

                Finalize();

                Log.Write("Error", $"Unhandled exception: {exception.Message} {exception.StackTrace}");
            };

            currentDomain.ProcessExit += (_, e) => 
            {
                Finalize();

                Log.Write("Finalized", "Flagged to end.");
            };
        }
    }
}
