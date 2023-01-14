﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StokKontrol_API.Entities.Entities
{
    public class Supplier : BaseEntity
    {
        public Supplier()
        {
            this.Urunler = new List<Product>();
        }

        public string SupplierName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }


        public virtual List<Product> Urunler { get; set; }
    }
}
