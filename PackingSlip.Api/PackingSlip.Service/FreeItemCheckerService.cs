using PackingSlip.Domain.Entities;
using PackingSlip.Domain.Models;
using PackingSlip.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Service
{
    public interface IFreeItemCheckerService
    {
        Task<PackingSlipHeader> CheckForFreeItems(PackingSlipHeader packingSlipHeader);
    }

    public class FreeItemCheckerService: IFreeItemCheckerService
    {
        private readonly IFreeProductRepository _freeProductRepository = null;

        public FreeItemCheckerService(IFreeProductRepository freeProductRepository)
        {
            _freeProductRepository = freeProductRepository;
        }

        public async Task<PackingSlipHeader> CheckForFreeItems(PackingSlipHeader packingSlipHeader)
        {
            var freeProducts = await _freeProductRepository.GetFreeProductList();

            if (freeProducts.Any())
            {
                freeProducts.ForEach(s =>
                {
                    packingSlipHeader = CheckAndAddFreeItems(packingSlipHeader, s);
                });
            }

            return packingSlipHeader;
        }

        private PackingSlipHeader CheckAndAddFreeItems(PackingSlipHeader packingSlipHeader, FreeProductHeaderModel headerModel)
        {
            bool isApplicable = true;

            foreach (var parent in headerModel.ParentProducts)
            {
                var productExists = (from tmpLine in packingSlipHeader.PackingSlipItems where tmpLine.Name == parent.Name && tmpLine.Quantity >= parent.Quantity select tmpLine).FirstOrDefault();
                if (productExists == null)
                {
                    isApplicable = false;
                    break;
                }
            }

            if (isApplicable)
            {
                foreach(var child in headerModel.ChildProducts)
                {
                    packingSlipHeader.PackingSlipItems.Add(new PackingSlipItem
                    {
                        IsFreeItem = true,
                        Name = child.Name,
                        ProductId = child.ProductId,
                        Quantity = child.Quantity
                    });
                }
            }

            return packingSlipHeader;
        }
    }
}
