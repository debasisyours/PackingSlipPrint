using Microsoft.EntityFrameworkCore;
using PackingSlip.Domain;
using PackingSlip.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackingSlip.Repository
{
    public interface IFreeProductRepository
    {
        Task<List<FreeProductHeaderModel>> GetFreeProductList();
    }

    public class FreeProductRepository: IFreeProductRepository
    {
        private readonly PackingDbContext _context = null;

        public FreeProductRepository(PackingDbContext context)
        {
            _context = context;
        }

        public async Task<List<FreeProductHeaderModel>> GetFreeProductList()
        {
            List<FreeProductHeaderModel> freeProductList = new List<FreeProductHeaderModel>();
            FreeProductHeaderModel freeProduct = null;

            var freeItems = await _context.FreeProducts.Where(s => s.IsActive).ToListAsync();
            if (freeItems.Any())
            {
                freeItems.ForEach(s =>
                {
                    freeProduct = new FreeProductHeaderModel();
                    var parents = _context.FreeProductParents.Where(x => x.FreeProductId == s.Id).ToList();
                    parents.ForEach(p =>
                    {
                        var parentProduct = _context.Products.FirstOrDefault(x => x.Id == p.ProductId);
                        freeProduct.ParentProducts.Add(new FreeProductDetailModel
                        {
                            ProductId = p.ProductId,
                            Name = parentProduct.Name,
                            Quantity = p.RequiredQuantity
                        });
                    });

                    var children = _context.FreeProductChildren.Where(x => x.FreeProductId == s.Id).ToList();
                    children.ForEach(p =>
                    {
                        var childProduct = _context.Products.FirstOrDefault(x => x.Id == p.ProductId);
                        freeProduct.ChildProducts.Add(new FreeProductDetailModel
                        {
                            ProductId = p.ProductId,
                            Name = childProduct.Name,
                            Quantity = p.FreeQuantity
                        });
                    });

                    freeProductList.Add(freeProduct);
                });
            }

            return freeProductList;
        }
    }
}
