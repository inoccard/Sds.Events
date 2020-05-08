namespace ProAgil.Domain.Entities
{
    public class SocialNetWork
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public int? EventId { get; set; }
        public Event Event { get; }
        public int? SpeakerId { get; set; }
        public Speaker Speraker { get; }
    }
}