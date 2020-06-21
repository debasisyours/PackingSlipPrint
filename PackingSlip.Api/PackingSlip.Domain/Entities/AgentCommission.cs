using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class AgentCommission
    {
        public int Id { get; set; }
        public string AgentName { get; set; }
        public decimal Amount { get; set; }
        public int PackingSlipId { get; set; }
    }
}
