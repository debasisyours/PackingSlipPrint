using System;

namespace PackingSlip.Utility
{
    public class PackingSlipConstants
    {
        public const string MembershipCreationSuccessMessage = "Membership created successfully.";
        public const string MembershipActivationSuccessMessage = "Membership activated successfully.";
        public const string MembershipUpgradeSuccessMessage = "Membership upgraded successfully.";
        public const string MembershipAlreadyExistsMessage = "Membership already exists.";
        public const string MembershipDoesNotExistMessage = "Membership not found.";
        public const string MembershipAlreadyActivatedMessage = "Membership already activated.";
        public const string MembershipAlreadyUpgradedMessage = "Membership already upgraded.";

        public const string CustomerEmailCannotBeBlankMessage = "Customer email should not be blank.";
        public const string AgentCannotBeBlankMessage = "Agent name required to generate commission payment.";
    }
}
