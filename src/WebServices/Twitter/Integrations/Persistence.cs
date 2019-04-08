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
            var collection =  new MongoClient("mongodb://localhost:27017").GetDatabase("processed-tweets").GetCollection<Analysis>("analysis");

            var analysis = collection.Find(a => 
                a.Subject == subject
            ).FirstOrDefault();

            var logMessage = analysis == null ? 
                $"Analysis from \"{date}\" and \"{subject}\" not found." :  
                $"Query analysis from \"{analysis.Date}\" and \"{analysis.Subject}\"";

            Log.Write("Persistence", logMessage);

            return analysis;
        }
    }
}