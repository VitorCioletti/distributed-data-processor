namespace Twitter.Entities
{

    using System;

    public class Analysis
    {
        public DateTime Date { get; set; }

        public int Messages { get; set; }

        public long Subjectivity { get; set; }

        public long Polarity { get; set; }

        public string Subject { get; set; }
    }
}