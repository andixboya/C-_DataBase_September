
namespace P01_StudentSystem.Data.Model_Configurations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;

    public class CourseConfig : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder
                .HasKey(c => c.CourseId);

            builder
                .Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode(true);

            builder
                .Property(c => c.Description)
                .IsUnicode(true)
                .IsRequired(false);
        }
    }
}
