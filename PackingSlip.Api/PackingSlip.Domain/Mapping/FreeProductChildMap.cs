using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class FreeProductChildMap : IEntityTypeConfiguration<FreeProductChild>
    {
        public void Configure(EntityTypeBuilder<FreeProductChild> builder)
        {
            builder.ToTable("dbo", "FreeProductChild");
            builder.HasKey(s => s.Id);
        }
    }
}
