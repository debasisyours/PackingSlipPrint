using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Entities
{
    public class CustomerMembership
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool IsActivated { get; set; }
        public DateTime? ActivatedOn { get; set; }
        public bool? IsUpgraded { get; set; }
        public DateTime? UpgradedOn { get; set; }
    }
}
