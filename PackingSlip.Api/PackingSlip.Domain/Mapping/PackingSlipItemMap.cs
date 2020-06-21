using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class PackingSlipItemMap : IEntityTypeConfiguration<PackingSlipItem>
    {
        public void Configure(EntityTypeBuilder<PackingSlipItem> builder)
        {
            builder.ToTable("dbo", "PackingSlipItem");
            builder.HasKey(s => s.Id);
        }
    }
}
