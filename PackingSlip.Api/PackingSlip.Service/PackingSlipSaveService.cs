using PackingSlip.Domain.Entities;
using PackingSlip.Repository;
using System;
using System.Threading.Tasks;

namespace PackingSlip.Service
{
    public interface IPackingSlipSaveService
    {
        Task<ResponseMessage> SavePackingSlip(PackingSlipHeader packingSlip);
    }

    public class PackingSlipSaveService: IPackingSlipSaveService
    {
        private readonly IPackingSlipRepository _packingSlipRepository = null;
        private readonly IPackingSlipValidator _packingSlipValidator = null;
        private readonly IFreeItemCheckerService _freeItemCheckerService = null;
        private readonly ICustomerMembershipRepository _customerMembershipRepository = null;

        public PackingSlipSaveService(IPackingSlipRepository packingSlipRepository, 
            IPackingSlipValidator packingSlipValidator,
            IFreeItemCheckerService freeItemCheckerService,
            ICustomerMembershipRepository customerMembershipRepository)
        {
            _packingSlipRepository = packingSlipRepository;
            _packingSlipValidator = packingSlipValidator;
            _freeItemCheckerService = freeItemCheckerService;
            _customerMembershipRepository = customerMembershipRepository;
        }

        public async Task<ResponseMessage> SavePackingSlip(PackingSlipHeader packingSlip)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = true };
            packingSlip = await _freeItemCheckerService.CheckForFreeItems(packingSlip);
            message = _packingSlipValidator.IsPackingSlipRequestValid(packingSlip);

            if (!message.IsSuccess)
            {
                return message;
            }

            message = await _packingSlipRepository.SavePackingSlipAsync(packingSlip);
            if (packingSlip.ActivateMembership.HasValue && packingSlip.ActivateMembership.Value) await _customerMembershipRepository.ActivateMembershipAsync(packingSlip.CustomerEmail);
            if (packingSlip.UpgradeMembership.HasValue && packingSlip.UpgradeMembership.Value) await _customerMembershipRepository.UpgradeMembershipAsync(packingSlip.CustomerEmail);

            return message;
        }
    }
}
