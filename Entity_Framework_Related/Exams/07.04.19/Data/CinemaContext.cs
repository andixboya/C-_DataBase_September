namespace Cinema.Data
{
    using Cinema.Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class CinemaContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Hall> Halls { get; set; }

        public DbSet<Movie> Movies { get; set; }

        public DbSet<Projection> Projections { get; set; }

        public DbSet<Seat> Seats { get; set; }

        public DbSet<Ticket> Tickets { get; set; }


        public CinemaContext() { }

        public CinemaContext(DbContextOptions options)
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

            modelBuilder.Entity<Movie>(m =>
            {
                m.HasKey(x => x.Id);
                m.Property(x => x.Title)
                .IsRequired(true);

                m.Property(x => x.Genre)
                .IsRequired(true);

                m.Property(x => x.Duration)
                .IsRequired(true);

                m.Property(x => x.Rating)
                .IsRequired(true);

                m.Property(x => x.Director)
                .IsRequired(true);
            });

            modelBuilder.Entity<Projection>(p =>
            {
                p.HasOne(x => x.Movie)
                .WithMany(m => m.Projections)
                .HasForeignKey(x => x.MovieId);

                p.HasOne(x => x.Hall)
                .WithMany(h => h.Projections)
                .HasForeignKey(x => x.HallId);
            });

            modelBuilder.Entity<Ticket>(t =>
            {
                t.HasOne(x => x.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(x => x.CustomerId);

                t.HasOne(x => x.Projection)
                .WithMany(p => p.Tickets)
                .HasForeignKey(x => x.ProjectionId);

            });

            modelBuilder.Entity<Seat>(s =>
            {
                s.HasOne(x => x.Hall)
                .WithMany(h => h.Seats)
                .HasForeignKey(x => x.HallId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}