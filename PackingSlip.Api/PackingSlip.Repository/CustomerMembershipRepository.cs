using PackingSlip.Domain;
using PackingSlip.Domain.Entities;
using PackingSlip.Utility;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PackingSlip.Repository
{
    public interface ICustomerMembershipRepository
    {
        Task<ResponseMessage> SaveMembershipAsync(CustomerMembership customerMembership);
        Task<ResponseMessage> ActivateMembershipAsync(string email);
        Task<ResponseMessage> UpgradeMembershipAsync(string email);
        ResponseMessage IsMembershipValidForCreate(string email);
        ResponseMessage IsMembershipValidForActivate(string email);
        ResponseMessage IsMembershipValidForUpgrade(string email);
    }

    public class CustomerMembershipRepository: ICustomerMembershipRepository
    {
        private readonly PackingDbContext _context = null;

        public CustomerMembershipRepository(PackingDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseMessage> SaveMembershipAsync(CustomerMembership customerMembership)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = false };
            customerMembership.CreatedOn = DateTime.Now;
            _context.CustomerMemberships.Add(customerMembership);
            await _context.SaveChangesAsync();
            
            message.IsSuccess = true;
            message.Detail = PackingSlipConstants.MembershipCreationSuccessMessage;
            return message;
        }

        public async Task<ResponseMessage> ActivateMembershipAsync(string email)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = false };
            var customerMembership = _context.CustomerMemberships.FirstOrDefault(s => s.Email == email);
            customerMembership.ActivatedOn = DateTime.Now;
            customerMembership.IsActivated = true;
            await _context.SaveChangesAsync();

            message.IsSuccess = true;
            message.Detail = PackingSlipConstants.MembershipActivationSuccessMessage;
            return message;
        }

        public async Task<ResponseMessage> UpgradeMembershipAsync(string email)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = false };
            var customerMembership = _context.CustomerMemberships.FirstOrDefault(s => s.Email == email);
            customerMembership.UpgradedOn = DateTime.Now;
            customerMembership.IsUpgraded = true;
            await _context.SaveChangesAsync();

            message.IsSuccess = true;
            message.Detail = PackingSlipConstants.MembershipUpgradeSuccessMessage;
            return message;
        }

        public ResponseMessage IsMembershipValidForCreate(string email)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = true };
            var membership = _context.CustomerMemberships.FirstOrDefault(s => s.Email == email);
            
            if (membership != null)
            {
                message.Detail = PackingSlipConstants.MembershipAlreadyExistsMessage;
                message.IsSuccess = false;
            }

            return message;
        }

        public ResponseMessage IsMembershipValidForActivate(string email)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = true };
            var membership = _context.CustomerMemberships.FirstOrDefault(s => s.Email == email);

            if (membership == null)
            {
                message.IsSuccess = false;
                message.Detail = PackingSlipConstants.MembershipDoesNotExistMessage;
            }
            else
            {
                if (membership.IsActivated)
                {
                    message.IsSuccess = false;
                    message.Detail = PackingSlipConstants.MembershipAlreadyActivatedMessage;
                }
            }

            return message;
        }

        public ResponseMessage IsMembershipValidForUpgrade(string email)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = true };
            var membership = _context.CustomerMemberships.FirstOrDefault(s => s.Email == email);

            if (membership == null)
            {
                message.IsSuccess = false;
                message.Detail = PackingSlipConstants.MembershipDoesNotExistMessage;
            }
            else
            {
                if (membership.IsUpgraded.HasValue && membership.IsUpgraded.Value)
                {
                    message.IsSuccess = false;
                    message.Detail = PackingSlipConstants.MembershipAlreadyUpgradedMessage;
                }
            }

            return message;
        }
    }
}
