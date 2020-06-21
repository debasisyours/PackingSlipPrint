using Microsoft.EntityFrameworkCore.Storage;
using PackingSlip.Domain;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Repository
{
    public interface IPackingSlipRepository
    {
        Task<ResponseMessage> SavePackingSlipAsync(PackingSlipHeader packingSlip);
    }

    public class PackingSlipRepository: IPackingSlipRepository
    {
        private readonly PackingDbContext _context = null;

        public PackingSlipRepository(PackingDbContext context)
        {
            _context = context;
        }

        public async Task<ResponseMessage> SavePackingSlipAsync(PackingSlipHeader packingSlip)
        {
            ResponseMessage message = new ResponseMessage { IsSuccess = false };
            int maxNumber = 0;

            using (IDbContextTransaction transaction = _context.Database.BeginTransaction())
            {
                if (_context.PackingSlipHeaders.Any())
                {
                    maxNumber = _context.PackingSlipHeaders.Max(s => s.Id);
                }

                packingSlip.PackingSlipNumber = $"PS-{maxNumber}";
                _context.PackingSlipHeaders.Add(packingSlip);
                await _context.SaveChangesAsync();

                int packingId = packingSlip.Id;
                packingSlip.PackingSlipItems.ForEach(s =>
                {
                    var product = _context.Products.FirstOrDefault(x => x.Name == s.Name);
                    _context.PackingSlipItems.Add(new PackingSlipItem
                    {
                        Amount = (s.IsFreeItem.HasValue && s.IsFreeItem.Value) ? 0 : s.Quantity * product.Rate,
                        Name = s.Name,
                        PackingSlipId = packingId,
                        Quantity = s.Quantity,
                        ProductId = product.Id,
                    });
                });
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                message.IsSuccess = true;
                message.Detail = packingSlip.PackingSlipNumber;
            }

            return message;
        }
    }
}
