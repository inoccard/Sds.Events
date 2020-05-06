using System;
namespace ProAgil.WebAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Local { get; set; }
        public string EventDate { get; set; }
        public string Theme { get; set; }
        public int PersonQtd { get; set; }
        public string Lot { get; set; }
    }
}