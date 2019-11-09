

namespace P01_StudentSystem.Data.Model_Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ResourceConfig : IEntityTypeConfiguration<Resource>
    {
        public void Configure(EntityTypeBuilder<Resource> builder)
        {

            builder
                .HasKey(r => r.ResourceId);

            builder
                .Property(r => r.Name)
                .HasMaxLength(50)
                .IsRequired(true)
                .IsUnicode(true);

            builder
                .Property(r => r.Url)
                .IsRequired(true)
                .IsUnicode(false);

            builder.HasOne(r => r.Course)
                .WithMany(c => c.Resources)
                .HasForeignKey(r => r.ResourceId);




        }
    }
}
