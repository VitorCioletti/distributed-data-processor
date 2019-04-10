namespace Twitter.Entities
{

    using System;
    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;

    public class Analysis
    {
        [BsonId]
        public ObjectId _id { get; set; }

        public string date { get; set; }

        public long messages { get; set; }

        public double subjectivity { get; set; }

        public double polarity { get; set; }

        public string subject { get; set; }
    }
}