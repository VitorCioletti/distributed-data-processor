namespace Twitter
{
    using System;
    using LLibrary;
    using static System.Console;

    public static class Log
    {
        private static L _log = new L();

        public static void Write(string label, string log)
        {
            _log.Log(label, log);

            WriteLine($"{DateTime.Now} {label.ToUpper()} - {log}");
        }

        public static void WriteException(string label, Exception e) => 
            Write(label, $"Exception: {e.Message} {e.StackTrace}");

        public static void WriteInitialized(Type t) => Write("Initialization", $"Initialized {t.Name}.");
    
        public static void WriteFinalized(Type t) => Write("Finalization", $"Finalized {t.Name}.");
    }
}