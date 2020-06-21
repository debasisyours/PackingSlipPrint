using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PackingSlip.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace PackingSlip.Domain.Mapping
{
    public class CustomerMembershipMap : IEntityTypeConfiguration<CustomerMembership>
    {
        public void Configure(EntityTypeBuilder<CustomerMembership> builder)
        {
            builder.ToTable("dbo", "CustomerMembership");
            builder.HasKey(s => s.Id);
        }
    }
}
