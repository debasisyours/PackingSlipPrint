using Microsoft.Extensions.Options;
using PackingSlip.Domain.Entities;
using PackingSlip.Repository;
using PackingSlip.Utility;
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
        private readonly IAgentCommissionRepository _agentCommissionRepository = null;
        private readonly IEmailService _emailService = null;
        private FileConfig _fileConfig = null;

        public PackingSlipSaveService(IPackingSlipRepository packingSlipRepository, 
            IPackingSlipValidator packingSlipValidator,
            IFreeItemCheckerService freeItemCheckerService,
            ICustomerMembershipRepository customerMembershipRepository,
            IAgentCommissionRepository agentCommissionRepository,
            IEmailService emailService,
            IOptions<FileConfig> options)
        {
            _packingSlipRepository = packingSlipRepository;
            _packingSlipValidator = packingSlipValidator;
            _freeItemCheckerService = freeItemCheckerService;
            _customerMembershipRepository = customerMembershipRepository;
            _agentCommissionRepository = agentCommissionRepository;
            _emailService = emailService;
            _fileConfig = options.Value;
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
            if((packingSlip.HasPhysicalProduct.HasValue && packingSlip.HasPhysicalProduct.Value)||
                    (packingSlip.HasBook.HasValue && packingSlip.HasBook.Value))
            {
                await _agentCommissionRepository.AddAgentCommission(packingSlip, 100);
            }

            if (packingSlip.ActivateMembership.HasValue && packingSlip.ActivateMembership.Value)
            {
                await _customerMembershipRepository.ActivateMembershipAsync(packingSlip.CustomerEmail);
                await _emailService.SendMail(_fileConfig.UserCredential, packingSlip.CustomerEmail,
                    PackingSlipConstants.MembershipActivationSubject, PackingSlipConstants.MembershipActivationBody,
                    _fileConfig.UserCredential, _fileConfig.Password, _fileConfig.Host, _fileConfig.Port);
            }

            if (packingSlip.UpgradeMembership.HasValue && packingSlip.UpgradeMembership.Value)
            {
                await _customerMembershipRepository.UpgradeMembershipAsync(packingSlip.CustomerEmail);
                await _emailService.SendMail(_fileConfig.UserCredential, packingSlip.CustomerEmail,
                    PackingSlipConstants.MembershipUpgradeSubject, PackingSlipConstants.MembershipUpgradeBody,
                    _fileConfig.UserCredential, _fileConfig.Password, _fileConfig.Host, _fileConfig.Port);
            }

            return message;
        }
    }
}
