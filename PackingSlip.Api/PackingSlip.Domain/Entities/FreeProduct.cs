using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class FreeProduct
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public virtual List<FreeProductParent> FreeProductParents { get; set; }
        public virtual List<FreeProductChild> FreeProductChildren { get; set; }

        public FreeProduct()
        {
            FreeProductChildren = new List<FreeProductChild>();
            FreeProductParents = new List<FreeProductParent>();
        }
    }
}
