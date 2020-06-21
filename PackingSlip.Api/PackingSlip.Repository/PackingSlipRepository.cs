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

            try
            {
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
                    foreach (var item in packingSlip.PackingSlipItems.ToList())
                    {
                        var product = _context.Products.FirstOrDefault(x => x.Name == item.Name);
                        _context.PackingSlipItems.Add(new PackingSlipItem
                        {
                            Amount = (item.IsFreeItem.HasValue && item.IsFreeItem.Value) ? 0 : item.Quantity * product.Rate,
                            Name = item.Name,
                            PackingSlipId = packingId,
                            Quantity = item.Quantity,
                            ProductId = product.Id,
                        });
                    }
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();

                    message.IsSuccess = true;
                    message.Detail = packingSlip.PackingSlipNumber;
                }
            }
            catch(Exception ex)
            {

            }

            return message;
        }
    }
}
