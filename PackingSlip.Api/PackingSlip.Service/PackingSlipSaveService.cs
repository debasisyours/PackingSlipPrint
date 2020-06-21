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

        public PackingSlipSaveService(IPackingSlipRepository packingSlipRepository)
        {
            _packingSlipRepository = packingSlipRepository;
        }

        public async Task<ResponseMessage> SavePackingSlip(PackingSlipHeader packingSlip)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = true };

            return message;
        }
    }
}
