﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class OrderDetails : BaseEntity
    {
        [ForeignKey("Siparis")]
        public int OrderId { get; set; }
        public virtual Order Siparis { get; set; }


        public decimal UnitPrice { get; set; }
        public short? Quantity { get; set; }


        [ForeignKey("Urun")]
        public int ProductId { get; set; }
        public virtual Product Urun { get; set; }

    }
}
