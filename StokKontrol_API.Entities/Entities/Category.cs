using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class Category : BaseEntity
    {
        public Category()
        {
            this.Products = new List<Product>();
        }

        public string CategoryName { get; set; }
        public string Description { get; set; }

        // Navigation Property
        // Bir kategorinin birden fazla ürünü olabilir
        public virtual List<Product> Products { get; set; }
    }
}
