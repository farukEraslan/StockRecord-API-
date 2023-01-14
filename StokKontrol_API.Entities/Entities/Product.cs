using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class Product : BaseEntity
    {
        public Product()
        {
            this.SiparisDetayı = new List<OrderDetails>();
        }

        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public short? Stock { get; set; }
        public DateTime? ExpireDate { get; set; }



        // Navigation properties
        [ForeignKey("Kategori")]
        public int CategoryId { get; set; }
        public virtual Category Kategori { get; set; }
        
        [ForeignKey("Tedarikci")]
        public int SupplierId { get; set; }
        public virtual Supplier Tedarikci { get; set; }


        public List<OrderDetails> SiparisDetayı { get; set; }
    }
}
