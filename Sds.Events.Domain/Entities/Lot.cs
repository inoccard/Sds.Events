using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sds.Events.Domain.Entities
{
    public class Lot
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int Qty { get; set; }
        public int EventId { get; set; }
        public Event Event { get; }

    }
}