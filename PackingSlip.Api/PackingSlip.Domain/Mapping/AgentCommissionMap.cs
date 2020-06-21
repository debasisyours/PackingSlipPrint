using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class AgentCommissionMap : IEntityTypeConfiguration<AgentCommission>
    {
        public void Configure(EntityTypeBuilder<AgentCommission> builder)
        {
            builder.ToTable("AgentCommission", "dbo");
            builder.HasKey(s => s.Id);
        }
    }
}
