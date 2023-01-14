using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class BaseEntity
    {
        [Column(Order = 1)]     // Bütün entitylerde bu kolon 1. sırada olacak şekilde ayarlandı.
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime AddedDate { get; set; } = DateTime.Now;
        public DateTime? ModifiedDate { get; set; }
    }
}
