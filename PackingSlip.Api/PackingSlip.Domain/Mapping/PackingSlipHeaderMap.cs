using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class PackingSlipHeaderMap : IEntityTypeConfiguration<PackingSlipHeader>
    {
        public void Configure(EntityTypeBuilder<PackingSlipHeader> builder)
        {
            builder.ToTable("dbo", "PackingSlipHeader");
            builder.HasKey(s => s.Id);
            builder.HasMany(s => s.PackingSlipItems)
                .WithOne(s => s.PackingSlipHeader)
                .HasForeignKey(s => s.PackingSlipId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}
