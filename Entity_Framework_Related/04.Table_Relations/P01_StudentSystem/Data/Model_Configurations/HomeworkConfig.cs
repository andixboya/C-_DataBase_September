

namespace P01_StudentSystem.Data.Model_Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using System;

    public class HomeworkConfig : IEntityTypeConfiguration<Homework>
    {
        public void Configure(EntityTypeBuilder<Homework> builder)
        {
            builder
                .HasKey(h => h.HomeworkId);

            builder
                .Property(h => h.Content)
                .IsRequired(true) //this may... be unnecessary  //TODO:
                .IsUnicode(false);

            builder
                .HasOne(h => h.Student)
                .WithMany(s => s.HomeworkSubmissions)
                .HasForeignKey(h => h.StudentId);

            builder
                .HasOne(h => h.Course)
                .WithMany(c => c.HomeworkSubmissions)
                .HasForeignKey(h => h.CourseId);

        }
    }
}
