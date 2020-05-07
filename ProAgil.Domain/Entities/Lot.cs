using System;
namespace ProAgil.Domain.Entities {
    public class Lot {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Qty { get; set; }
        public int EventiId { get; set; }
        public Event Event { get; set; }
        
    }
}