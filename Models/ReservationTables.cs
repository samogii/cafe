using System.ComponentModel.DataAnnotations.Schema;

namespace Cafe.Models
{
    public class ReservationTables
    {
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        [ForeignKey("ReserveTable")]
        public int TableId { get; set; }
        public virtual ReserveTable Table { get; set; }
        public virtual User User { get; set; }
        public DateTime ReserveDate { get; set; }
    }
}
