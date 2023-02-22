﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations
{
    public class EventCategoryConfiguration : IEntityTypeConfiguration<EventCategory>
    {
        public void Configure(EntityTypeBuilder<EventCategory> builder)
        {
            builder.HasMany(ec => ec.CategorizedEvents)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.EventCategoryId)
                   .HasConstraintName("FK_EVENT_CATEGORY_ID")
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
