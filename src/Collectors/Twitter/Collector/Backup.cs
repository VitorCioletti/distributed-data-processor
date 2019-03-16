namespace Twitter
{
    using System;
    using System.Collections.Generic;

    public static class Backup
    {
        public static event Func<IEnumerable<object>, bool> TrySend;

        public static void Initialize()
        {

        }

        public static void Finalize()
        {
            
        }

        public static void Write(IEnumerable<Tweet> messages)
        {
            
        }

        private static void Clear(IEnumerable<string> messages)
        {

        }
    }
}