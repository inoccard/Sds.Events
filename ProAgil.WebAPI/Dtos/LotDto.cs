using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sds.Events.WebAPI.Dtos
{
    public class LotDto
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Column (TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        public DateTime? InitDate { get; set; }
        public DateTime? EndDate { get; set; }

        [RequiredAttribute]
        public int Qty { get; set; }
    }
}