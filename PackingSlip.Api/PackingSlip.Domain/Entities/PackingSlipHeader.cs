using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class PackingSlipHeader
    {
        public int Id { get; set; }
        public string PackingSlipNumber { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string AgentName { get; set; }
        public bool? HasPhysicalProduct { get; set; }
        public bool? HasBook { get; set; }
        public bool? ActivateMembership { get; set; }
        public bool? UpgradeMembership { get; set; }
        public virtual List<PackingSlipItem> PackingSlipItems { get; set; }

        public PackingSlipHeader()
        {
            PackingSlipItems = new List<PackingSlipItem>();
        }
    }
}
