using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class FreeProductParentMap : IEntityTypeConfiguration<FreeProductParent>
    {
        public void Configure(EntityTypeBuilder<FreeProductParent> builder)
        {
            builder.ToTable("dbo", "FreeProductParent");
            builder.HasKey(s => s.Id);
        }
    }
}
