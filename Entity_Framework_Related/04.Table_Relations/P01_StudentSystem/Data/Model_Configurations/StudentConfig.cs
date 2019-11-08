

namespace P01_StudentSystem.Data.Model_Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using P01_StudentSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Text;
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {

            builder.HasKey(s => s.StudentId);

            builder
                .Property(s => s.Name)
                .HasMaxLength(100)
                .IsRequired(true)
                .IsUnicode(true);

            builder
                .Property(s => s.PhoneNumber)
                .HasColumnType("VARCHAR(10)")
                .IsRequired(true);

            builder.Property(s => s.Birthday)
                .IsRequired(false);

        }
    }
}
