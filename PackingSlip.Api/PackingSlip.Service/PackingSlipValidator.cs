using PackingSlip.Domain.Entities;
using PackingSlip.Repository;
using PackingSlip.Utility;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Service
{
    public interface IPackingSlipValidator
    {
        ResponseMessage IsPackingSlipRequestValid(PackingSlipHeader packingSlip);
    }

    public class PackingSlipValidator: IPackingSlipValidator
    {
        private readonly ICustomerMembershipRepository _customerMembershipRepository = null;

        public PackingSlipValidator(ICustomerMembershipRepository customerMembershipRepository)
        {
            _customerMembershipRepository = customerMembershipRepository;
        }

        public ResponseMessage IsPackingSlipRequestValid(PackingSlipHeader packingSlip)
        {
            ResponseMessage response = new ResponseMessage { IsSuccess = true };

            if (string.IsNullOrEmpty(packingSlip.CustomerEmail))
            {
                response.IsSuccess = false;
                response.Detail = PackingSlipConstants.CustomerEmailCannotBeBlankMessage;
                return response;
            }

            response = _customerMembershipRepository.IsMembershipValidForCreate(packingSlip.CustomerEmail);
            if (response.IsSuccess)
            {
                response = _customerMembershipRepository.SaveMembershipAsync(new CustomerMembership
                {
                    Email = packingSlip.CustomerEmail,
                    IsActivated = false,
                    IsUpgraded = false
                }).Result;
            }
            else
            {
                response.IsSuccess = true;
            }

            if((packingSlip.HasPhysicalProduct.HasValue && packingSlip.HasPhysicalProduct.Value)||
                (packingSlip.HasBook.HasValue && packingSlip.HasBook.Value))
            {
                if (string.IsNullOrEmpty(packingSlip.AgentName))
                {
                    response.IsSuccess = false;
                    response.Detail = PackingSlipConstants.AgentCannotBeBlankMessage;
                    return response;
                }
            }

            if (packingSlip.ActivateMembership.HasValue && packingSlip.ActivateMembership.Value)
            {
                response = _customerMembershipRepository.IsMembershipValidForActivate(packingSlip.CustomerEmail);
                if (!response.IsSuccess) return response;
            }

            if (packingSlip.UpgradeMembership.HasValue && packingSlip.UpgradeMembership.Value)
            {
                response = _customerMembershipRepository.IsMembershipValidForUpgrade(packingSlip.CustomerEmail);
                if (!response.IsSuccess) return response;
            }

            return response;
        }
    }
}
