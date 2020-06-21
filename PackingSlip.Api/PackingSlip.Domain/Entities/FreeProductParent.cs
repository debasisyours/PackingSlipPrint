using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class FreeProductParent
    {
        public int Id { get; set; }
        public int FreeProductId { get; set; }
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
        public virtual FreeProduct FreeProduct { get; set; }
    }
}
