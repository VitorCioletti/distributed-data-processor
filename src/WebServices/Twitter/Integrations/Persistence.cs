namespace Twitter.Integrations
{
    using System;
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Logging;

    public static class Persistence
    {
        public static Analysis GetAnalysis(DateTime date, string subject)
        {
            var collection =  new MongoClient().GetDatabase("processed-tweets").GetCollection<Analysis>("analysis");

            var analysis = collection.Find(x => x.Date == date && x.Subject == subject).Single();

            Log.Write("Persistence", $"Query analysis from \"{analysis.Date}\" and \"{analysis.Subject}\"");

            return analysis;
        }
    }
}