namespace Collector
{
    using System;
    using System.Threading.Tasks;

    public class Collector
    {
        public event Action<string> Send;

        public void Initialize()
        {
            Log.Write("Collector","Initialized collector.");
        }

        public void Collect()
        {
            Task.Run(
                () =>
                {
                    Send("Sample message.");
                }
            ).ContinueWith(
                (e) => Log.Write("Collector", $"{e.Exception.Message} {e.Exception.StackTrace}"),
                TaskContinuationOptions.OnlyOnFaulted
            );
        }

        public void Finalize()
        {
            Log.Write("Collector", "Finalized collector.");
        }
    }
}