namespace Twitter
{
    using System;
    
    public class Tweet : Message
    {
        public string Subject { get; set; }

        public string IdCreator { get; set; }        
    }
}