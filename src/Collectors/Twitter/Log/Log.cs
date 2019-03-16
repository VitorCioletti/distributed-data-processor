namespace Twitter
{
    using System;
    using LLibrary;

    public static class Log
    {
        private static L _log = new L();

        public static void Write(string label, string log) => _log.Log(label, log);
    }
}