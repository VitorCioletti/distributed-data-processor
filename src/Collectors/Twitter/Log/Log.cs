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
    }
}