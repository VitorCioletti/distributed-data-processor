namespace Collector
{
    using System;
    using LLibrary;

    public static class Log
    {
        private static L _log;

        public static void Initialize() => _log = new L();

        public static void Write(string label, string log) 
        {
            if (_log == null)
                throw new Exception($"Log {_log} not initialized.");

            _log.Log(label, log);
        }
    }
}