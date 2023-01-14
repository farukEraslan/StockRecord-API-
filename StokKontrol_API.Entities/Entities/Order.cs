using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class Order : BaseEntity
    {
        public Order()
        {
            this.SiparisDetayı = new List<OrderDetails>();
        }

        [ForeignKey("Kullanici")]
        public int UserId { get; set; }
        public virtual User Kullanici { get; set; }


        public virtual List<OrderDetails> SiparisDetayı { get; set; }
    }
}
