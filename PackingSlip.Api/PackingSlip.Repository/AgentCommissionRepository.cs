using PackingSlip.Domain;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Repository
{
    public interface IAgentCommissionRepository
    {
        Task<ResponseMessage> AddAgentCommission(PackingSlipHeader packingSlipHeader, decimal amount);
    }

    public class AgentCommissionRepository: IAgentCommissionRepository
    {
        private readonly PackingDbContext _context = null;

        public AgentCommissionRepository(PackingDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseMessage> AddAgentCommission(PackingSlipHeader packingSlipHeader, decimal amount)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = false };

            var commission = new AgentCommission
            {
                AgentName = packingSlipHeader.AgentName,
                Amount = amount,
                PackingSlipId = packingSlipHeader.Id
            };

            _context.AgentCommissions.Add(commission);
            await _context.SaveChangesAsync();
            message.IsSuccess = true;

            return message;
        }
    }
}
