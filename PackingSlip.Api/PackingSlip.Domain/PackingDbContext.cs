using Microsoft.EntityFrameworkCore;
using PackingSlip.Domain.Entities;
using PackingSlip.Domain.Mapping;
using System;

namespace PackingSlip.Domain
{
    public class PackingDbContext:DbContext
    {
        public PackingDbContext(DbContextOptions<PackingDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration<CustomerMembership>(new CustomerMembershipMap());
            modelBuilder.ApplyConfiguration<FreeProduct>(new FreeProductMap());
            modelBuilder.ApplyConfiguration<FreeProductParent>(new FreeProductParentMap());
            modelBuilder.ApplyConfiguration<FreeProductChild>(new FreeProductChildMap());
            modelBuilder.ApplyConfiguration<Product>(new ProductMap());
            modelBuilder.ApplyConfiguration<PackingSlipHeader>(new PackingSlipHeaderMap());
            modelBuilder.ApplyConfiguration<PackingSlipItem>(new PackingSlipItemMap());
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<FreeProduct> FreeProducts { get; set; }
        public DbSet<FreeProductParent> FreeProductParents { get; set; }
        public DbSet<FreeProductChild> FreeProductChildren { get; set; }
        public DbSet<CustomerMembership> CustomerMemberships { get; set; }
        public DbSet<PackingSlipHeader> PackingSlipHeaders { get; set; }
        public DbSet<PackingSlipItem> PackingSlipItems { get; set; }
    }
}
