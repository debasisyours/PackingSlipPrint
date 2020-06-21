using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Sku { get; set; }
        public decimal Rate { get; set; }
    }
}
