using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Models
{
    public class FreeProductDetailModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
    }
}
