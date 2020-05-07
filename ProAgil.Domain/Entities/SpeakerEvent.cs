namespace ProAgil.Domain.Entities
{
    public class SpeakerEvent
    {
        public int Id { get; set; }
        public int SpeakerId { get; set; }
        public Speaker Spreaker { get; set; }
        public int EventId { get; set; }
        public Event Event { get; set; }
    }
}