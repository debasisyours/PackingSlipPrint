using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class FreeProductMap : IEntityTypeConfiguration<FreeProduct>
    {
        public void Configure(EntityTypeBuilder<FreeProduct> builder)
        {
            builder.ToTable("FreeProduct", "dbo");
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.FreeProductParents)
                .WithOne(s => s.FreeProduct)
                .HasForeignKey(s => s.FreeProductId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasMany(s => s.FreeProductChildren)
                .WithOne(s => s.FreeProduct)
                .HasForeignKey(s => s.FreeProductId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
