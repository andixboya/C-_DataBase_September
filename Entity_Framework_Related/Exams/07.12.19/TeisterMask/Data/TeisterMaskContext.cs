namespace TeisterMask.Data
{
    using Microsoft.EntityFrameworkCore;
    using TeisterMask.Data.Models;

    public class TeisterMaskContext : DbContext
    {

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Task> Tasks { get; set; }

        public DbSet<EmployeeTask> EmployeesTasks { get; set; }



        public TeisterMaskContext() { }

        public TeisterMaskContext(DbContextOptions options)
            : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(Configuration.ConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Task>(t =>
            {
                t.HasOne(x => x.Project)
                .WithMany(p => p.Tasks)
                .HasForeignKey(x => x.ProjectId);

            });

            modelBuilder.Entity<EmployeeTask>(et =>
            {
                et.HasKey(x => new { x.EmployeeId, x.TaskId });

                et.HasOne(x => x.Task)
                .WithMany(t => t.EmployeesTasks)
                .HasForeignKey(x => x.TaskId);

                et.HasOne(x => x.Employee)
                .WithMany(e => e.EmployeesTasks)
                .HasForeignKey(x => x.EmployeeId);

            });
        }
    }
}