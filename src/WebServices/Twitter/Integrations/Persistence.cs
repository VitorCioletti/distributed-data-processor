namespace Twitter.Integrations
{
    using System;
    using Entities;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using Logging;

    public static class Persistence
    {
        public static Analysis GetAnalysis(string date, string subject)
        {
            var client =  new MongoClient("mongodb://localhost:27017");
            
            var db = client.GetDatabase("processed-tweets");
            
            var collection = db.GetCollection<Analysis>("analysis");

            // Somehow this mongodb library can't convert Date...
            var analysis = collection.Find<Analysis>(a => a.subject == subject && a.date == date).FirstOrDefault();

            var logMessage = analysis == null ? 
                $"Analysis from \"{date}\" and \"{subject}\" not found." :  
                $"Query analysis from \"{analysis.date}\" and \"{analysis.subject}\"";

            Log.Write("Persistence", logMessage);

            return analysis;
        }
    }
}