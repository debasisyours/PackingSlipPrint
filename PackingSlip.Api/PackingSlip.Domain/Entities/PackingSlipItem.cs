using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class PackingSlipItem
    {
        public int Id { get; set; }
        public int PackingSlipId { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public bool? IsFreeItem { get; set; }
        public virtual PackingSlipHeader PackingSlipHeader { get; set; }
    }
}
